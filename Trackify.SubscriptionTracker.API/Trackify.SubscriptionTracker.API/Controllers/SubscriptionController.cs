using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trackify.SubscriptionTracker.Application.Dtos;
using Trackify.SubscriptionTracker.Application.SubscriptionCommand;
using Trackify.SubscriptionTracker.Application.SubscriptionQuery;

namespace Trackify.SubscriptionTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SubscriptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionCommand command)
        {

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("User not logged in.");
            }

            command.UserId = int.Parse(userIdString);

            await _mediator.Send(command);

            return Ok(new { message = "Subscription created successfully" });

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubscription([FromRoute] int id, [FromBody] UpdateSubscriptionCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok(new { message = "Subscription updated successfully" });
        }

        [HttpPatch("update-status/{id}")]
        public async Task<IActionResult> UpdateSubscriptionStatus([FromRoute] int id, [FromBody] UpdateSubscriptionStatusCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok(new { message = "Subscription status updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription([FromRoute] int id)
        {
            DeleteSubscriptionCommand command = new DeleteSubscriptionCommand();
            command.Id = id;
            await _mediator.Send(command);
            return Ok(new { message = "Subscription deleted successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubscription()
        {

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("User not logged in.");
            }

            var query = new GetAllSubscriptionQuery
            {
                UserId = int.Parse(userIdString)
            };

            SubscriptionResponseDto subscriptions = await _mediator.Send(query);
            return Ok(subscriptions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubscriptionById([FromRoute] int id)
        {
            SubscriptionGetByIdQuery query = new SubscriptionGetByIdQuery();
            query.Id = id;

            var subscription = await _mediator.Send(query);
            return Ok(subscription);
        }

    }
}
