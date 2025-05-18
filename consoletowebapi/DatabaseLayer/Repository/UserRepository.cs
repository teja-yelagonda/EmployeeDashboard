using consoletowebapi.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace consoletowebapi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly OrganizationContext _context;
        public UserRepository(OrganizationContext context)
        {
            _context = context;
        }
        public int UpdateOtp(string userName, string otp)
        {
            Employee empDetails = _context.Employees.FirstOrDefault(x => x.Email == userName);
            User existingUser = _context.Users.FirstOrDefault(x => x.EmployeeId == empDetails.Id);

            existingUser.ResetOtp = int.Parse(otp);
            existingUser.OtpExpirytime = DateTime.Now.AddMinutes(10);
            existingUser.IsOtpused= false;
            _context.Users.Update(existingUser);
            return _context.SaveChanges();
        }

        public async Task<bool> ValidateUser(int EmpId)
        {
            User existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == EmpId);
            if (existingUser == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> ValidateOTP(int OTP, int EmpId)
        {
            User existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == EmpId);
            if ((existingUser.ResetOtp == OTP) && (existingUser.OtpExpirytime > DateTime.Now) && (existingUser.IsOtpused == false))
            {
                existingUser.IsOtpused = true;
                _context.Update(existingUser);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdatePassword(string newPassword, int EmpId)
        {
            User existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == EmpId);
            if (existingUser == null)
            {
                return false;
            }
            existingUser.Password = newPassword;
            _context.Update(existingUser);
            _context.SaveChanges();
            return true;
        }
        public int AddRefreshToken(int empId,string refreshToken)
        {
            User existingUser = _context.Users.FirstOrDefault(x => x.EmployeeId == empId);
            existingUser.RefreshToken = refreshToken;
            existingUser.RefreshTokenExpityTime = DateTime.Now.AddDays(7);
            _context.Users.Update(existingUser);
            int count = _context.SaveChanges();
            return count;
        }
    }
}
