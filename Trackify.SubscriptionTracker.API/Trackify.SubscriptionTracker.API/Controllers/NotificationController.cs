using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trackify.SubscriptionTracker.Application.NotificationCommand;
using Trackify.SubscriptionTracker.Application.NotificationDto;
using Trackify.SubscriptionTracker.Application.NotificationQuery;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Trackify.SubscriptionTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotifications()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("User not logged in.");
            }

            var query = new GetAllNotificationsQuery
            {
                UserId = int.Parse(userIdString)
            };

            var userNotifications = await _mediator.Send(query);
            return Ok(userNotifications);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateNotification([FromBody] UpdateNotificationCommand command)
        {
            if (command.NotificationIds == null || command.NotificationIds.Count == 0)
            {
                return BadRequest("No notification IDs provided.");
            }

            var count = await _mediator.Send(command);
            return Ok(new { message = $"{count} notifications were marked as read" });
        }
    }
}
