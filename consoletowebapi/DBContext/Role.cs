using System;
using System.Collections.Generic;

#nullable disable

namespace consoletowebapi.DBContext
{
    public partial class Role
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Role1 { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
