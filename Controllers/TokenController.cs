using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestApiCRUDDemo.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestApiCRUDDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly BootCampDemoContext _context;

        public TokenController(IConfiguration configuration, BootCampDemoContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserInfo userInfo)
        {
            if(userInfo != null && userInfo.Email != null && userInfo.Password != null)
            {
                var user = await GetUser(userInfo.Email, userInfo.Password);

                if(user != null)
                {

                    // create claims details based on the user information

                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                       new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                       new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                       new Claim("Id", user.UserId.ToString()),
                       new Claim("FirstName",user.FirstName),
                        new Claim("LastName",user.LastName),
                         new Claim("UserName",user.UserName),
                          new Claim("Email",user.Email)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"],claims,expires: DateTime.UtcNow.AddDays(1),signingCredentials:signin);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid Credentials");
                }
            }
            else
            {
                return BadRequest();
            }
           
        }

        private async Task<UserInfo> GetUser(string email, string password)
        {
            return await _context.UserInfo.FirstOrDefaultAsync(u=>u.Email == email && u.Password == password);
        }
    }
}
