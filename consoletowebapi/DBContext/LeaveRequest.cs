using System;
using System.Collections.Generic;

#nullable disable

namespace consoletowebapi.DBContext
{
    public partial class LeaveRequest
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int ManagerId { get; set; }
        public string ManagerEmail { get; set; }
        public int LeaveType { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Status { get; set; }
        public DateTime? ApprovalDate { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
