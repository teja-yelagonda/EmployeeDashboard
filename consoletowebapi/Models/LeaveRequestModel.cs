using consoletowebapi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace consoletowebapi.Models
{
    public class LeaveRequestModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int ManagerId { get; set; }
        public string ManagerEmail { get; set; }
        public LeaveType LeaveType { get; set; } = LeaveType.StandardLeave;
        public DateTime? RequestDate { get; set; } = DateTime.Now;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public LeaveStatus Status { get; set; }=LeaveStatus.Pending;
        public DateTime? ApprovalDate { get; set; } = null;
    }
}
