using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trackify.SubscriptionTracker.Application.Users.Command;

namespace Trackify.SubscriptionTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(CreateUserCommand command)
        {
            var newUser = await _mediator.Send(command);

            if (newUser != null)
            {
                return BadRequest();
            }
            return Ok();    
        }
    }
}
