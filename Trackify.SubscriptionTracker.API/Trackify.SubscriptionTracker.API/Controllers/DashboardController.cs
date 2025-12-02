using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trackify.SubscriptionTracker.Application.DashBoard.Dtos;
using Trackify.SubscriptionTracker.Application.DashBoard.Query;

namespace Trackify.SubscriptionTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("User not logged in.");
            }
            GetAllDashBoardDataQuery getAllDashBoardDataQuery = new() {
                UserId= int.Parse(userIdString)
            };
            DashboardDto response =await _mediator.Send(getAllDashBoardDataQuery);
            return Ok(response);
        }
    }
}
