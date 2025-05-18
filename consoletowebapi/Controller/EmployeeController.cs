using consoletowebapi.DTO;
using consoletowebapi.Models;
using consoletowebapi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace consoletowebapi.Controller
{
    [ApiController]
    [Route("api/[Controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("GetEmployees")]
        public ActionResult<EmployeeDTO> GetEmpoyees()
        {
            List<EmployeeDTO> employees=_employeeService.GetEmployees();
            return Ok(employees);
        }

        [HttpGet("GetEmployeeById")]
        public ActionResult<EmployeeDTO> GetEmpoyeeById(int Id)
        {
            if (Id < 0)
            {
                return BadRequest("Id should not be 0 or negative");
            }
            EmployeeDTO employeeDetails = _employeeService.GetEmployeeById(Id);
            if (employeeDetails == null)
            {
                return NotFound("Employee Details not found");
            }
            return employeeDetails;
        }

        //[HttpPost("AddEmployee")]
        //public ActionResult<List<EmployeeDTO>> AddEmployee([FromBody] List<EmployeeDTO> Employees)
        //{
        //    if (Employees == null)
        //    {
        //        return BadRequest("Employee Details should not be null");
        //    }
        //    int count= _employeeService.AddEmployee(Employees);
        //    if (count > 0)
        //    {
        //        return CreatedAtAction(nameof(GetEmpoyeeById), null, Employees);
        //    }
        //    return StatusCode(500, "unable to save details in Database");
        //}

        [HttpPut("EditEmployee")]
        public IActionResult EditEmployee(EmployeeModel Employees)
        {
            if (Employees == null)
            {
                return BadRequest("EmployeeDetails can not be null");
            }
            int count = _employeeService.EditEmployee(Employees);
            if (count > 0)
            {
                return Ok("Updated the Details");
            }
            return StatusCode(500, "unable to update details in Database");
        }

        [HttpDelete("DeleteEmployee/{Id}")]
        public IActionResult DeleteEmployee([FromRoute] int Id)
        {
            if (Id < 0)
            {
                return BadRequest(new { message = "Id value cannot be 0 or negative" });
            }
            bool IsDeleted = _employeeService.DeleteEmployee(Id);
            if (IsDeleted == true)
            {
                return NoContent();
            }
            return StatusCode(500, $"Unable to delete employee Details");
        }
        [HttpGet("DownloadEmployeeDetails")]
        public IActionResult DownloadExcel()
        {
            List<EmployeeDTO> Employees = _employeeService.GetEmployees();
            var filebytes = _employeeService.DownloadExcel(Employees);
            return File(filebytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EmployeeDetails.xlsx");
        }
        
        [HttpPost("UploadDetailsFromExcel")]
        public IActionResult UploadDetailsFromExcel(IFormFile file)
        {
            if(file==null || file.Length == 0)
            {
                return BadRequest(new { message = "Please upload a valid file." });
            }
            _employeeService.UploadDetailsFromExcel(file);
            
                return Ok();
        }
    }
}
