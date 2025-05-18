using consoletowebapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace consoletowebapi.Services
{
    public interface ILeaveRequestService
    {
        int RequestLeave(LeaveRequestModel leaveRequest);
        //void SendEmailReminder(LeaveRequestModel leave);
    }
}
