using consoletowebapi.BusinessLayer.IServices;
using consoletowebapi.Models;
using consoletowebapi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace consoletowebapi.Controller
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmployeeService _employeeService;
        private readonly IUserService _userService;

        private readonly IEmailService _emailService;
        public AuthController(IConfiguration configuration, IEmployeeService employeeService, IEmailService emailService, IUserService userService)
        {
            _configuration = configuration;
            _employeeService = employeeService;
            _emailService = emailService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel user)
        {
            if (!_employeeService.IsUserExist(user.UserName))
            {
                return BadRequest("User with given phoneNumber or Email doesnot exist");
            }
            string password = _employeeService.GetPassword(user.UserName);
            string role = _employeeService.GetRole(user.UserName);
            int empId = _employeeService.GetEmployeeId(user.UserName);

            if (password == _employeeService.HashPassword(user.Password))
            {
                string token = _userService.GenerateJwtToken(user.UserName, role);
                string refreshToken = _userService.GenerateRefreshToken();
                int count = _userService.AddRefreshToken(empId, refreshToken);
                return Ok(new { token });
            }
            return Unauthorized("Username or Password is incorrect");
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegistrationModel newUser)
        {
            if ((!_employeeService.IsUserExist(newUser.Email)) || (!_employeeService.IsUserExist(Convert.ToString(newUser.PhoneNumber))))
            {
                _employeeService.RegisterUser(newUser);

                return Ok(new { message = "User Registration successfull" });
            }
            return BadRequest("Username Already Exist");
        }

        [AllowAnonymous]
        [HttpGet("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string userName)
        {
            if (_employeeService.IsUserExist(userName))
            {
                int isMailSent = await _emailService.ForgorPassword(userName);
                if (isMailSent > 0)
                {
                    return Ok(new { message = "Sent OTP to the registered Email address" });
                }
                return StatusCode(500, new { message = "failed to send Email" });
            }
            return BadRequest("UserName NotFound");
        }

        [AllowAnonymous]
        [HttpPost("ValidateOTP")]
        public async Task<IActionResult> ValidateOTP([FromBody] OTPValidation otpValidation)
        {
            if (_employeeService.IsUserExist(otpValidation.Email))
            {
                int EmpId = _employeeService.GetEmployeeId(otpValidation.Email);
                if (await _emailService.ValidateUser(EmpId))
                {
                    if(await _emailService.ValidateOTP(otpValidation.OTP, EmpId))
                    {
                        return Ok(new { message = "OTP validated" });
                    }
                    return BadRequest("Invalid OTP");
                }
                return BadRequest("User NotFound");
            }
            return BadRequest("Employee NotFound");
        }

        [AllowAnonymous]
        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordUpdate updatePassword)
        {
            if (_employeeService.IsUserExist(updatePassword.Email))
            {
                int EmpId = _employeeService.GetEmployeeId(updatePassword.Email);
                if (await _emailService.ValidateUser(EmpId))
                {
                    if (await _emailService.UpdatePassword(updatePassword.Password, EmpId))
                    {
                        return Ok(new { message = "Password Updated Successfully" });
                    }
                }
                return BadRequest("User NotFound");
            }
            return BadRequest("Employee NotFound");
        }
    }
}
