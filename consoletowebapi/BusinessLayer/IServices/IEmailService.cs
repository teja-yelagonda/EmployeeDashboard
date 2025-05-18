using consoletowebapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace consoletowebapi.Services
{
    public interface IEmailService
    {
        Task<int> ForgorPassword(string userName);
        Task<bool> ValidateUser(int username);
        Task<bool> ValidateOTP(int OTP, int EmpId);
        Task<bool> UpdatePassword(string Password, int EmpId);
        bool SendEmailReminder(LeaveRequestModel leave);
    }
}
