using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Application.Notification.Query;
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
        public async Task<IActionResult> RefreshToken()
        {
            RefreshTokenCommand refreshTokenCommand = new RefreshTokenCommand();
            var response = await _mediator.Send(refreshTokenCommand);
            if (response == null)
            {
                return BadRequest(new { message = "failed to refresh token" });
            }
            return Ok(response);
        }

        [Authorize(Roles = "Subscriber")]
        [HttpGet("validate")]
        public async Task<IActionResult> Validate()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("User not logged in.");
            }

            ValidateUserQuery query = new ValidateUserQuery
            {
                UserId = int.Parse(userIdString)
            };
            GetUserDto user = await _mediator.Send(query);
            return Ok(user);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            LogoutCommand command = new();
            var logoutResponse = await _mediator.Send(command);
            return Ok(new { message = logoutResponse });
        }
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody]CreateOtpCommand command)
        {
            string otp =await _mediator.Send(command);
            if (otp == null)
            {
                return BadRequest(new { message = "OTP generation failed" });
            }

            Console.WriteLine("OTP sent successfully."+otp);

            return Ok(new { message = "OTP sent successfully. Please check your email."});
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpCommand command)
        {
            bool response = await _mediator.Send(command);
            if (!response) { return BadRequest(new { message = "OTP verification failed" }); }
            return Ok(new { message = "OTP verfication successfully" });
        }
    }
}
