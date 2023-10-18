using System;
using System.Collections.Generic;

namespace RestApiCRUDDemo.Models
{
    public partial class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public int Age { get; set; }
    }
}
