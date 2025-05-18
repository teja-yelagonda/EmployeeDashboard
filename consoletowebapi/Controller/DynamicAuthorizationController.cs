using consoletowebapi.Models;
using consoletowebapi.Services;
using Microsoft.AspNetCore.Mvc;

namespace consoletowebapi.Controller
{
    [ApiController]
    [Route("api/[Controller]")]
    public class DynamicAuthorizationController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public DynamicAuthorizationController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpPost("UpdateEndpointRoles")]
       public IActionResult UpdateEndpointRoles([FromBody] UpdateEndPointRoles updatedRoles)
       {
            int rowsEffected=_employeeService.UpdateEndpointRoles(updatedRoles);
            if (rowsEffected > 0)
            {
                return Ok();
            }
            return StatusCode(500, new { message = "unable to update roles please try again" });
       }
    }
}
