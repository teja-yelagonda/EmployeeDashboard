using System;
using System.Collections.Generic;

#nullable disable

namespace consoletowebapi.DBContext
{
    public partial class Employee
    {
        public Employee()
        {
            LeaveRequests = new HashSet<LeaveRequest>();
            Roles = new HashSet<Role>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public long PhoneNumber { get; set; }
        public string Address { get; set; }

        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
