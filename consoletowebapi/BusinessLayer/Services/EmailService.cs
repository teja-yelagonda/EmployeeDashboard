using consoletowebapi.Models;
using consoletowebapi.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace consoletowebapi.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeService _employeeService;
        public EmailService(IConfiguration configuration, IUserRepository userRepository, IEmployeeService employeeService)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _employeeService = employeeService;
        }
        public async Task<int> ForgorPassword(string userName)
        {
            string otp = GenerateOtp();
            bool isMailSent = await sendOtpEmail(userName, otp);
            if (isMailSent)
            {
                return _userRepository.UpdateOtp(userName, otp);
            }
            return 0;
        }
        public static string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
        public async Task<bool> sendOtpEmail(string userName, string otp)
        {
            IConfigurationSection emailSettings = _configuration.GetSection("SmtpSettings");
            string FromAddress = emailSettings["FromAddress"];
            string FromName = emailSettings["FromName"];
            string smtpHost = emailSettings["SMTPHost"];

            SmtpClient client = new SmtpClient(smtpHost);

            MailAddress fromAddress = new MailAddress(FromAddress, FromName, System.Text.Encoding.UTF8);
            MailAddress toAddress = new MailAddress(userName);

            MailMessage message = new MailMessage()
            {
                From = fromAddress,
                Subject = "OTP Generation for password reset"
                //Body = $"Dear User<br>Your otp for password reset is: {otp}<br>This OTP valid for 10 minutes<br><br>Thanks,<br>Organization.",
            };
            message.To.Add(toAddress);
            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            client.UseDefaultCredentials = true;
            //message.Body = "<body style =\"font-family: Calibri\">";
            //message.Body += $"Dear User<br><br>Your otp for password reset is: {otp}<br>This OTP valid until 10 minutes<br><br>Thanks,<br>Organization.";

            string CurrentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(CurrentDirectory, "HTMLFiles\\PasswordResetEmail.html");

            string emailBody = File.ReadAllText(filePath);
            emailBody = emailBody.Replace("{{OTP}}", Convert.ToString(otp));
            message.Body = emailBody;


            try
            {
                client.Send(message);
                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error sending Email: {error.Message}");
                return false;
            }
        }

        public async Task<bool> ValidateUser(int userId)
        {
            return await _userRepository.ValidateUser(userId);
        }

        public async Task<bool> ValidateOTP(int OTP, int EmpId)
        {
            return await _userRepository.ValidateOTP(OTP, EmpId);
        }

        public async Task<bool> UpdatePassword(string password, int empId)
        {
            string hashedPassword = _employeeService.HashPassword(password);
            return await _userRepository.UpdatePassword(hashedPassword, empId);
        }

        public bool SendEmailReminder(LeaveRequestModel leave)
        {
            IConfigurationSection emailSettings = _configuration.GetSection("SmtpSettings");
            string FromAddress = emailSettings["FromAddress"];
            string FromName = emailSettings["FromName"];
            string smtpHost = emailSettings["SMTPHost"];

            SmtpClient client = new SmtpClient(smtpHost);

            MailAddress fromAddress = new MailAddress(FromAddress, FromName, System.Text.Encoding.UTF8);
            MailAddress toAddress = new MailAddress(leave.ManagerEmail);

            MailMessage message = new MailMessage()
            {
                From = fromAddress,
                Subject = "Remainder Email for employees leave approval",
                IsBodyHtml = true,
                BodyEncoding = System.Text.Encoding.UTF8,
                SubjectEncoding = System.Text.Encoding.UTF8
            };
            message.To.Add(toAddress);
            client.UseDefaultCredentials = true;
            
            string CurrentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(CurrentDirectory, "HTMLFiles\\ReminderEmail.html");
            
            string emailBody = File.ReadAllText(filePath);
            string managerName = _employeeService.GetEmployeeName(leave.ManagerId);
            emailBody=emailBody.Replace("{{ManagerName}}",Convert.ToString(managerName));
            emailBody = emailBody.Replace("{{EmployeeName}}", Convert.ToString(leave.EmployeeName));
            emailBody = emailBody.Replace("{{LeaveStatus}}", Convert.ToString(leave.Status));
            emailBody = emailBody.Replace("{{LeaveType}}", Convert.ToString(leave.LeaveType));
            emailBody = emailBody.Replace("{{StartDate}}", Convert.ToString(leave.StartDate));
            emailBody = emailBody.Replace("{{EndDate}}", Convert.ToString(leave.EndDate));
            message.Body = emailBody;

            try
            {
                client.Send(message);
                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error sending Email: {error.Message}");
                return false;
            }
        }
    }
}
