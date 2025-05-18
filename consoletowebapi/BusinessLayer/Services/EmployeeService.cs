using AutoMapper;
using ClosedXML.Excel;
using consoletowebapi.DatabaseLayer.IRepository;
using consoletowebapi.DBContext;
using consoletowebapi.DTO;
using consoletowebapi.Models;
using consoletowebapi.Repository;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace consoletowebapi.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISwaggerEndpointsRepository _swaggerEndpointsRepository;
        private readonly IMapper _mapper;
        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, ISwaggerEndpointsRepository swaggerEndpointsRepository)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _swaggerEndpointsRepository = swaggerEndpointsRepository;
        }

        public List<EmployeeDTO> GetEmployees()
        {
            List<Employee> employees= _employeeRepository.GetEmployees();
            List<EmployeeDTO> employeeDTO = _mapper.Map<List<EmployeeDTO>>(employees);
            return employeeDTO;
        }
        public EmployeeDTO GetEmployeeById(int Id)
        {
            Employee employee=_employeeRepository.GetEmployeeById(Id);
            EmployeeDTO employeeDTO = _mapper.Map<EmployeeDTO>(employee);
            return employeeDTO;
        }
        //public int AddEmployee(List<EmployeeDTO> Employees)
        //{
        //    return _employeeRepository.AddEmployee(Employees);
        //}
        public int EditEmployee(EmployeeModel Employees)
        {
            int rowsEffected= _employeeRepository.EditEmployee(Employees);
            return rowsEffected;
        }
        public bool DeleteEmployee(int Id)
        {
            return _employeeRepository.DeleteEmployee(Id);
        }
        public string GetPassword(string userName)
        {
            return _employeeRepository.GetPassword(userName);
        }
        public string GetRole(string userName)
        {
            return _employeeRepository.GetRole(userName);
        }
        public int RegisterUser(RegistrationModel newUser)
        {
            newUser.Password = HashPassword(newUser.Password);
            return _employeeRepository.RegisterUser(newUser);
        }
        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        public bool IsUserExist(string userName)
        {
            return _employeeRepository.IsUserExist(userName);
        }
        public int GetEmployeeId(string username)
        {
            return _employeeRepository.GetEmployeeId(username);
        }
        public string GetEmployeeName(int empId)
        {
            Employee employee=_employeeRepository.GetEmployeeById(empId);
            string FullName = employee.FirstName + employee.LastName;
            return FullName;
        }
        public byte[] DownloadExcel(List<EmployeeDTO> employees)
        {
            using (var package = new ExcelPackage())
            {
                List<EmployeeDTO> Employees2 = GetEmployees();
                var workSheet = package.Workbook.Worksheets.Add("EmployeeDetails");
                workSheet.Cells["A1"].LoadFromCollection(employees, true);
                workSheet.Cells.AutoFitColumns();
                var fileBytes = package.GetAsByteArray();
                return fileBytes;
            }
        }
        public void UploadDetailsFromExcel(IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                file.CopyToAsync(stream);
                using (XLWorkbook workbook =new XLWorkbook(stream))
                {
                    IXLWorksheet worksheet = workbook.Worksheet(1);
                    IXLRangeRows rows = worksheet.RangeUsed().RowsUsed();


                    List<WorkerDTO> workers = new List<WorkerDTO>();
                    foreach(var row in rows.Skip(1))
                    {
                        WorkerDTO worker = new WorkerDTO()
                        {
                            FirstName = row.Cell(1).Value.ToString(),
                            LastName = row.Cell(2).Value.ToString(),
                            Gender = row.Cell(3).Value.ToString()
                        };
                        workers.Add(worker);
                    }

                    List<Worker> mappedWorkers = _mapper.Map<List<Worker>>(workers);
                    _employeeRepository.InsertIntoDataBase(mappedWorkers);
                }
            }
        }
        public int UpdateEndpointRoles(UpdateEndPointRoles updateRoles)
        {
            int rowsEffected=_swaggerEndpointsRepository.UpdateEndpointRoles(updateRoles);
            return rowsEffected;
        }
    }
}
