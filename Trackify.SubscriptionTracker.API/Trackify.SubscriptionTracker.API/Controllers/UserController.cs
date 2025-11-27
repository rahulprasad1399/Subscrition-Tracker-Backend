using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trackify.SubscriptionTracker.Application.Users.Command;
using Trackify.SubscriptionTracker.Application.Users.Dto;
using Trackify.SubscriptionTracker.Application.Users.Query;

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

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserCommand command)
        {
            var userId = await _mediator.Send(command);

            return Ok(new { Id = userId, Message = "User created successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserCommand command)
        {
            LoginResponse loginResponse = await _mediator.Send(command);
            return Ok(loginResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UpdateUserCommand command)
        {
            command.Id = id;
            UpdateUser updatedUser = await _mediator.Send(command);
            return Ok(updatedUser);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            GetAllUserQuery getAllUserQuery = new GetAllUserQuery();
            List<GetUserDto> updateUsers = await _mediator.Send(getAllUserQuery);
            return Ok(updateUsers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByUserId([FromRoute] int id)
        {
            GetByIdUserQuery getByIdUserQuery = new()
            {
                Id = id
            };

            GetUserDto user = await _mediator.Send(getByIdUserQuery);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            DeleteUserCommand command = new DeleteUserCommand();
            command.Id = id;

            await _mediator.Send(command);
            return Ok(new { message = "User deleted successfully" });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommand refreshTokenCommand)
        {
            var response = await _mediator.Send(refreshTokenCommand);
            if (response == null)
            {
                return BadRequest(new { message = "failed to refresh token" });
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("validate")]
        public IActionResult Validate()
        {
            return Ok(new { authenticated = true });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            LogoutCommand command = new LogoutCommand();
            var logoutResponse = await _mediator.Send(command);
            return Ok(new { message = logoutResponse });
        }
    }
}
