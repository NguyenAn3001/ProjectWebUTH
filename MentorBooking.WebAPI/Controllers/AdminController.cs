using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentorBooking.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet("numbers-mentor")]
        public async Task<IActionResult> NumbersOfMentor()
        {
            var countMentor = await _adminService.GetAllMentor();
            return Ok(countMentor.Count());
        }
        [HttpGet("numbers-student")]
        public async Task<IActionResult> NumbersOfStudent()
        {
            var countStudent = await _adminService.GetAllStudent();
            return Ok(countStudent.Count());
        }
        [HttpGet("get-all-student")]
        public async Task<IActionResult> GetAllStudent()
        {
            var allStudent=await _adminService.GetAllStudent();
            return Ok(allStudent);
        }
        [HttpGet("get-all-mentor")]
        public async Task<IActionResult> GetAllMentor()
        {
            var allMentor=await _adminService.GetAllMentor();
            return Ok(allMentor);
        }
        [HttpGet("get-all-point-trans")]
        public async Task<IActionResult> GetAllPointTransaction()
        {
            var allPointTrans = _adminService.GetAllPointTransactions();
            return Ok(allPointTrans);
        }
        [HttpGet("get-all-session")]
        public async Task<IActionResult> GetAllSession()
        {
            var allSession = _adminService.GetAllSessions();
            return Ok(allSession);
        }
        [HttpDelete("delete-session")]
        public async Task<IActionResult> DeleteSession(Guid SessionId)
        {
            if (SessionId == Guid.Empty)
                return BadRequest(new { message = "SessionId is required" });
            var deleteSession = await _adminService.DeleteSession(SessionId);
            return deleteSession.Status switch
            {
                "Error" => BadRequest(new
                {
                    Status = deleteSession.Status,
                    Message = deleteSession.Message
                }),
                _ => Ok(new { Status = deleteSession.Status, Message =deleteSession.Message})
            };
        }
        [HttpDelete("delete-Transaction")]
        public async Task<IActionResult> DeletePointTrans(Guid TransactionId)
        {
            if (TransactionId == Guid.Empty)
                return BadRequest(new { message = "TrấnctionId is required" });
            var deleteTrans = await _adminService.DeletePointTransaction(TransactionId);
            return deleteTrans.Status switch
            {
                "Error" => BadRequest(new
                {
                    Status = deleteTrans.Status,
                    Message = deleteTrans.Message
                }),
                _ => Ok(new { Status = deleteTrans.Status, Message = deleteTrans.Message })
            };
        }
    }
}
