using AutoMapper;
using consoletowebapi.DatabaseLayer.IRepository;
using consoletowebapi.DBContext;
using consoletowebapi.DTO;
using consoletowebapi.Repository;
using consoletowebapi.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace consoletowebapi.xUnitTests.EmployeeTests
{
    public class EmployeeServiceTests
    {
        private readonly EmployeeService _empService;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IEmployeeRepository> _emprepo;
        private readonly List<Employee> _employees;
        private readonly Mock<ISwaggerEndpointsRepository> _swaggerrepo;
        public EmployeeServiceTests()
        {
            _emprepo = new Mock<IEmployeeRepository>();
            _swaggerrepo = new Mock<ISwaggerEndpointsRepository>();
            _mapper = new Mock<IMapper>();
            _empService = new EmployeeService(_emprepo.Object, _mapper.Object, _swaggerrepo.Object);

            List<Employee> _employees = new List<Employee>
            {
                new Employee{ Id=1,FirstName="Teja", LastName="Yelagonda",Gender="Male",Dob=DateTime.Parse("06/22/1999"),Address="Warangal",Email="tejayelagonda@gmail.com",PhoneNumber=6303632427 },
                new Employee{ Id=2,FirstName="Sumanth", LastName="Gorishetti",Gender="Male",Dob=DateTime.Parse("06/22/1999"),Address="Warangal",Email="tejayelagonda@gmail.com",PhoneNumber=6303632427 }
            };

        }
        [Fact]
        public void GetEmployees()
        {
            List<EmployeeDTO> employeeDTO = new List<EmployeeDTO>
            {
                new EmployeeDTO{ Id=1,FirstName="Teja", LastName="Yelagonda",Gender="Male",Dob=DateTime.Parse("06/22/1999"),Address="Warangal",Email="tejayelagonda@gmail.com",PhoneNumber=6303632427 },
                new EmployeeDTO{ Id=2,FirstName="Sumanth", LastName="Gorishetti",Gender="Male",Dob=DateTime.Parse("06/22/1999"),Address="Warangal",Email="tejayelagonda@gmail.com",PhoneNumber=6303632427 }
            };
            _emprepo.Setup(repo => repo.GetEmployees()).Returns(_employees);
            _mapper.Setup(mapper => mapper.Map<List<EmployeeDTO>>(_employees)).Returns(employeeDTO);


            var result = _empService.GetEmployees();
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Teja", result.First().FirstName);
        }
        [Fact]
        public void GetEmployeesById_ReturnsCorrectEmp()
        { 
            int empId = 1;
            Employee employee = new Employee { Id = 1, FirstName = "Teja", LastName = "Yelagonda", Gender = "Male", Dob = DateTime.Parse("06/22/1999"), Address = "Warangal", Email = "tejayelagonda@gmail.com", PhoneNumber = 6303632427 };
            _emprepo.Setup(repo => repo.GetEmployeeById(empId)).Returns(employee);

            EmployeeDTO empDTO = new EmployeeDTO { Id = 1, FirstName = "Teja", LastName = "Yelagonda", Gender = "Male", Dob = DateTime.Parse("06/22/1999"), Address = "Warangal", Email = "tejayelagonda@gmail.com", PhoneNumber = 6303632427 };
            var k = _mapper.Setup(mapper => mapper.Map<EmployeeDTO>(employee)).Returns(empDTO);


            var result = _empService.GetEmployeeById(empId);
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Teja", result.FirstName);
        }
        [Theory]
        [InlineData(1,true)]
        [InlineData(99, false)]
        public void GetEmployeesById_ReturnsCorrectData(int empId,bool IsEmpExist)
        {
            Employee employee = new Employee { Id = 1, FirstName = "Teja", LastName = "Yelagonda", Gender = "Male", Dob = DateTime.Parse("06/22/1999"), Address = "Warangal", Email = "tejayelagonda@gmail.com", PhoneNumber = 6303632427 };
            _emprepo.Setup(repo => repo.GetEmployeeById(empId)).Returns(employee);
                        
            if (IsEmpExist)
            {
                EmployeeDTO empDTO = new EmployeeDTO { Id = 1, FirstName = "Teja", LastName = "Yelagonda", Gender = "Male", Dob = DateTime.Parse("06/22/1999"), Address = "Warangal", Email = "tejayelagonda@gmail.com", PhoneNumber = 6303632427 };
                var k = _mapper.Setup(mapper => mapper.Map<EmployeeDTO>(employee)).Returns(empDTO);

                var result = _empService.GetEmployeeById(empId);

                Assert.NotNull(result);
                Assert.Equal(empId, result.Id);
                Assert.Equal("Teja", result.FirstName);
            }
            else
            {
                EmployeeDTO empDTO = null;
                var k = _mapper.Setup(mapper => mapper.Map<EmployeeDTO>(employee)).Returns(empDTO);

                var result = _empService.GetEmployeeById(empId);
                Assert.Null(result);
            }

            
        }
    }
}
