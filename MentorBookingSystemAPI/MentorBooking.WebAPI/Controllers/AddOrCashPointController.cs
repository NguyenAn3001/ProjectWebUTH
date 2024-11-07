using System.Security.Claims;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MentorBooking.WebAPI.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
[Route("api/wallet")]
public class AddOrCashPointController : ControllerBase
{
    private readonly IPointService _pointService;

    public AddOrCashPointController(IPointService pointService)
    {
        _pointService = pointService;
    }
    [Authorize]
    [HttpPost("add-point")]
    public async Task<IActionResult> AddPointToWallet(PointChangedModelRequest pointAddedRequest)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) });
        }
        var addResponse = await _pointService.AddPointAsync(Guid.Parse(userId), pointAddedRequest);
        return addResponse.Status switch
        {
            "Error" => BadRequest(new
            {
                status = addResponse.Status,
                message = addResponse.Message
            }),
            _ => Ok(addResponse)
        };
    }
    [Authorize]
    [HttpPost("cash-point")]
    public async Task<IActionResult> CashOutPoint(PointCashModelRequest pointCashRequest)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) });
        }
        var cashOutResponse = await _pointService.CashPointAsync(Guid.Parse(userId), pointCashRequest);
        return cashOutResponse.Status switch
        {
            "Error" => BadRequest(new
            {
                status = cashOutResponse.Status,
                message = cashOutResponse.Message
            }),
            _ => Ok(cashOutResponse)
        };
    }
}