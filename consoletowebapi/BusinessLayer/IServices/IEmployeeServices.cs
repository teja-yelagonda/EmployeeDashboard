using consoletowebapi.DBContext;
using consoletowebapi.DTO;
using consoletowebapi.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace consoletowebapi.Services
{
    public interface IEmployeeService
    {
        List<EmployeeDTO> GetEmployees();
        EmployeeDTO GetEmployeeById(int Id);
        int GetEmployeeId(string username);
        string GetEmployeeName(int empId);
        //int AddEmployee(List<EmployeeDTO> Employees);
        int EditEmployee(EmployeeModel Employees);
        bool DeleteEmployee(int Id);
        string GetPassword(string userName);
        string GetRole(string userName);
        string HashPassword(string password);
        int RegisterUser(RegistrationModel newUser);
        bool IsUserExist(string userName);
        byte[] DownloadExcel(List<EmployeeDTO> Employees);
        void UploadDetailsFromExcel(IFormFile file);
        int UpdateEndpointRoles(UpdateEndPointRoles updateRoles);
    }
}
