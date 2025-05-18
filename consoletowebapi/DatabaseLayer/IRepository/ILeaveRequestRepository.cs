using consoletowebapi.DBContext;
using consoletowebapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace consoletowebapi.Repository
{
    public interface ILeaveRequestRepository
    {
        int RequestLeave(LeaveRequest leaveRequest);
        List<LeaveRequest> GetPendingLeaveRequest();
    }
}
