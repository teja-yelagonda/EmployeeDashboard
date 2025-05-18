using consoletowebapi.DBContext;
using consoletowebapi.Repository;
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
    public class EmployeeReposioryTests
    {
        private readonly EmployeeRepository _employeeRepo;
        private readonly Mock<OrganizationContext> _dbContext;
        private readonly Mock<DbSet<Employee>> _dbSet;
        public EmployeeReposioryTests()
        {
            _dbContext = new Mock<OrganizationContext>();
            var employees = new List<Employee>
            {
                new Employee{ Id=1,FirstName="Teja", LastName="Yelagonda",Gender="Male",Dob=DateTime.Parse("06/22/1999"),Address="Warangal",Email="tejayelagonda@gmail.com",PhoneNumber=6303632427 },
                new Employee{ Id=2,FirstName="Sumanth", LastName="Gorishetti",Gender="Male",Dob=DateTime.Parse("06/22/1999"),Address="Warangal",Email="tejayelagonda@gmail.com",PhoneNumber=6303632427 }
            }.AsQueryable();

            _dbSet = new Mock<DbSet<Employee>>();
            _dbSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(employees.Provider);
            _dbSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(employees.Expression);
            _dbSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(employees.ElementType);
            _dbSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(employees.GetEnumerator());

            _dbContext.Setup(db => db.Employees).Returns(_dbSet.Object);

            _employeeRepo = new EmployeeRepository(_dbContext.Object);
        }
        [Fact]
        public void GetEmployees_ReturnsAllUsers()
        {
            List<Employee> result = _employeeRepo.GetEmployees();
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetEmployeeById_ReturnsCorrectUser()
        {
            int userId = 2;
            Employee result = _employeeRepo.GetEmployeeById(userId);
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
        }
        [Fact]
        public void GetEmployeeById_ReturnsNotFoundUser()
        {
            int userId = 99;
            Employee result = _employeeRepo.GetEmployeeById(userId);
            Assert.Null(result);
        }
    }
}
