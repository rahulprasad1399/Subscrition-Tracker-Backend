using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trackify.SubscriptionTracker.Application.Notification.Query;
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

        [HttpPatch("all-notification")]
        public async Task<IActionResult> UpdateNotification([FromBody] UpdateNotificationCommand command)
        {
            if (command.NotificationIds == null || command.NotificationIds.Count == 0)
            {
                return BadRequest("No notification IDs provided.");
            }

            var count = await _mediator.Send(command);
            return Ok(new { message = $"{count} notifications were marked as read" });
        }

        [HttpGet("unread-notification-count")]
        public async Task<IActionResult> GetUnReadNotificationCount()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("User not logged in.");
            }

            var query = new GetNewNotificationCount
            {
                UserId = int.Parse(userIdString)
            };

            var unreadNotificationCount = await _mediator.Send(query);
            return Ok(new { count = unreadNotificationCount });
        }

        [HttpPatch("update-notification/{id}")]
        public async Task<IActionResult> UpdateNotificationById([FromRoute] int id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("User not logged in.");
            }

            var query = new UpdateNotificationCommandById
            {
                UserId = int.Parse(userIdString),
                Id = id
            };

            var count = await _mediator.Send(query);
            return Ok(new { message = $"{count} notification marked as read" });
        }
    }
}
