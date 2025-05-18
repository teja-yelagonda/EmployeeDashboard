using consoletowebapi.DBContext;
using consoletowebapi.DTO;
using consoletowebapi.Models;
using System.Collections.Generic;
using System.Data;

namespace consoletowebapi.Repository
{
    public interface IEmployeeRepository
    {
        List<Employee> GetEmployees();
        Employee GetEmployeeById(int Id);
        int GetEmployeeId(string username);
        //int AddEmployee(List<EmployeeDTO> Employees);
        int EditEmployee(EmployeeModel Employees);
        bool DeleteEmployee(int Id);
        string GetPassword(string userName);
        //bool IsEmail(string userName);
        //bool IsPhoneNumber(string userName);
        string GetRole(string userName);
        int RegisterUser(RegistrationModel newUser);
        bool IsUserExist(string userName);
        void InsertIntoDataBase(List<Worker> workers);
    }
}
