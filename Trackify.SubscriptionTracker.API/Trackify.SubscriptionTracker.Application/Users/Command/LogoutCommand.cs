using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.Users.Command
{
    public class LogoutCommand : IRequest<string>
    {

    }

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, string>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IGenericRepository<User> _genericRepository;

        public LogoutCommandHandler(IHttpContextAccessor httpContextAccessor, IGenericRepository<User> genericRepository)
        {
            _contextAccessor = httpContextAccessor;
            _genericRepository = genericRepository;
        }
        public async Task<string> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var httpContext = _contextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new Exception("No active session");
            }

            var userIdString = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                throw new Exception("Cannot find user id");
            }

            var userId = int.Parse(userIdString);

            var user = await _genericRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("Cannot find user with this id");
            }

            user.AddRefreshToken(null, null);

            await _genericRepository.UpdateAsync(user);

            var deleteOption = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(-1)
            };

            httpContext.Response.Cookies.Delete("token", deleteOption);
            httpContext.Response.Cookies.Delete("refreshToken", deleteOption);

            return "Logged out successfully";
        }
    }
}
