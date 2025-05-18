using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace consoletowebapi.Models
{
    public class EmployeeModel
    {
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public long PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
