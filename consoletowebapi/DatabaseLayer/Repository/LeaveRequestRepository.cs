using consoletowebapi.DBContext;
using consoletowebapi.Enums;
using consoletowebapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace consoletowebapi.Repository
{
    public class LeaveRequestRepository:ILeaveRequestRepository
    {
        private readonly OrganizationContext _context;
        public LeaveRequestRepository(OrganizationContext context)
        {
            _context = context;
        }
        public int RequestLeave(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Add(leaveRequest);
            int rowsEffected=_context.SaveChanges();
            return rowsEffected;
        }
        public List<LeaveRequest> GetPendingLeaveRequest()
        {
            List<LeaveRequest> pendingLeaves= _context.LeaveRequests.Where(x => x.Status == (int)LeaveStatus.Pending).ToList();
            return pendingLeaves;
        }
    }
}
