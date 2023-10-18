using System;
using System.Collections.Generic;

namespace RestApiCRUDDemo.Models
{
    public partial class TblCustomer
    {
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string AddressText { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
