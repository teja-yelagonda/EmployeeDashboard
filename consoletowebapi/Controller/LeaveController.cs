using consoletowebapi.Models;
using consoletowebapi.Services;
using Microsoft.AspNetCore.Mvc;

namespace consoletowebapi.Controller
{
    [ApiController]
    [Route("api/[Controller]")]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;
        public LeaveController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }
        [HttpPost("RequestForLeave")]
        public IActionResult RequestLeave(LeaveRequestModel leaveRequest)
        {
            int rowsEffected=_leaveRequestService.RequestLeave(leaveRequest);
            if (rowsEffected == 0)
            {
                return StatusCode(500, new { message = "an unexpected error occured. Please try again" });
            }
            return Ok();
        }
    }
}
