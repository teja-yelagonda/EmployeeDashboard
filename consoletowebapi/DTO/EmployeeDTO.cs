using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace consoletowebapi.DTO
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public long PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
