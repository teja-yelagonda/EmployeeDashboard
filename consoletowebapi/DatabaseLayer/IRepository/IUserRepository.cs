using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace consoletowebapi.Repository
{
    public interface IUserRepository
    {
        int UpdateOtp(string userName, string otp);
        Task<bool> ValidateUser(int EmpId);
        Task<bool> ValidateOTP(int OTP, int EmpId);
        Task<bool> UpdatePassword(string Password, int EmpId);
        int AddRefreshToken(int empId,string refreshToken);
    }
}
