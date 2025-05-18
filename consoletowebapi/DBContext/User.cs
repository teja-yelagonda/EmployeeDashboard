using System;
using System.Collections.Generic;

#nullable disable

namespace consoletowebapi.DBContext
{
    public partial class User
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Password { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpityTime { get; set; }
        public int? ResetOtp { get; set; }
        public DateTime? OtpExpirytime { get; set; }
        public bool? IsOtpused { get; set; }
    }
}
