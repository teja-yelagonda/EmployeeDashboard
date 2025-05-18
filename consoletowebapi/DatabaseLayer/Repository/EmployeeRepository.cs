using consoletowebapi.DBContext;
using consoletowebapi.DTO;
using consoletowebapi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace consoletowebapi.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly OrganizationContext _context;
        public EmployeeRepository(OrganizationContext context)
        {
            _context = context;
        }
        public List<Employee> GetEmployees()
        {
            return _context.Employees.ToList();
        }
        public Employee GetEmployeeById(int Id)
        {
            Employee emp= _context.Employees.FirstOrDefault(x => x.Id == Id);
            return emp;
        }
        
        //public int AddEmployee(List<EmployeeDTO> Employees)
        //{
        //    foreach(EmployeeDTO employee in Employees)
        //    {
        //        _context.Employees.Add(new Employee 
        //                                { 
        //                                    Name=employee.Name
        //                                });
        //    }
        //    int x=_context.SaveChanges();
        //    return x;
        //}
        public int EditEmployee(EmployeeModel newEmployees)
        {
            int empId = GetEmpId(Convert.ToString(newEmployees.PhoneNumber));

            if (empId == 0)
            {
                return empId;
            }

            Employee existingEmp = GetEmployeeById(empId);

            existingEmp.FirstName = newEmployees.FirstName;
            existingEmp.LastName = newEmployees.Lastname;
            existingEmp.Dob = newEmployees.DOB;
            existingEmp.Gender = newEmployees.Gender;
            existingEmp.Email = newEmployees.Email;
            existingEmp.PhoneNumber = newEmployees.PhoneNumber;
            existingEmp.Address = newEmployees.Address;

            _context.Employees.Update(existingEmp);
            int x = _context.SaveChanges();
            return x;
        }

        public bool DeleteEmployee(int Id)
        {
            Employee existingEmployee = _context.Employees.Find(Id);
            User employeeCredentials = _context.Users.FirstOrDefault(x => x.EmployeeId == Id);
            Role employeeRole = _context.Roles.FirstOrDefault(x => x.EmployeeId == Id);

            if (existingEmployee == null || employeeCredentials == null || employeeRole == null)
            {
                return false;
            }

            _context.Users.Remove(employeeCredentials);
            _context.Roles.Remove(employeeRole);
            _context.Employees.Remove(existingEmployee);

            if (_context.SaveChanges() > 0)
            {
                return true;
            };
            return false;
        }
        public string GetPassword(string userName)
        {
            int empId = GetEmpId(userName);
            return _context.Users.FirstOrDefault(x => x.EmployeeId == empId).Password;
        }
        public string GetRole(string userName)
        {
            int empId = GetEmpId(userName);
            return _context.Roles.FirstOrDefault(x => x.EmployeeId == empId).Role1;
        }
        public int GetEmpId(string userName)
        {
            List<Employee> employees = _context.Employees.ToList();

            int empId;
            if (long.TryParse(userName, out long parsedUserName))
            {
                empId = employees.FirstOrDefault(x => x.PhoneNumber == parsedUserName)?.Id ??0;
                return empId;
            }
            empId= employees.FirstOrDefault(x => x.Email == userName).Id;
            return empId;
        }
        //public bool IsEmail(string userName)
        //{
        //    Regex emailRegrex = new Regex("");
        //    return emailRegrex.IsMatch(userName);
        //}
        //public bool IsPhoneNumber(string userName)
        //{
        //    Regex phoneRegex = new Regex(@"^\d{10}$");
        //    return phoneRegex.IsMatch(userName);
        //}
        public int RegisterUser(RegistrationModel newUser)
        {
            Employee employeeDetails = new Employee
            {
                FirstName = newUser.FirstName,
                LastName = newUser.Lastname,
                Dob = newUser.DOB,
                Gender = newUser.Gender,
                Email = newUser.Email,
                PhoneNumber = newUser.PhoneNumber,
                Address = newUser.Address
            };
            _context.Employees.Add(employeeDetails);
            _context.SaveChanges();

            int newEmpId = GetEmpId(Convert.ToString(newUser.PhoneNumber));

            User userDetails = new User
            {
                EmployeeId = newEmpId,
                Password = newUser.Password
            };

            Role newUserRole = new Role
            {
                EmployeeId = newEmpId,
                Role1 = "Employee"
            };

            _context.Users.Add(userDetails);
            _context.Roles.Add(newUserRole);
            var count = _context.SaveChanges();

            return count;
        }
        public bool IsUserExist(string userName)
        {
            if (long.TryParse(userName, out long parsedUserName))
            {
                return _context.Employees.Any(x => x.PhoneNumber == parsedUserName);
            }
            return _context.Employees.Any(x => x.Email == userName);
        }

        public int GetEmployeeId(string username)
        {
            return _context.Employees.FirstOrDefault(x => x.Email == username).Id;
        }
        public void InsertIntoDataBase(List<Worker> workers)
        {

            _context.Workers.AddRange(workers);
            _context.SaveChanges();
        }
    }
}
