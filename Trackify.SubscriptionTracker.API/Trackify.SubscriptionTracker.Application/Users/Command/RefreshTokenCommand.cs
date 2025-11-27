using MediatR;
using Microsoft.AspNetCore.Http;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Application.Users.Dto;

namespace Trackify.SubscriptionTracker.Application.Users.Command
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResponse>
    {

    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAuthRepository _authRepository;
        private readonly IJwtService _jwtService;
        public RefreshTokenCommandHandler(IHttpContextAccessor httpContextAccessor,
            IAuthRepository authRepository, IJwtService jwtService)
        {
            _contextAccessor = httpContextAccessor;
            _authRepository = authRepository;
            _jwtService = jwtService;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var httpContext = _contextAccessor.HttpContext;
            var refreshToken = httpContext?.Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new Exception("Refresh token not found");
            }

            var user = await _authRepository.ValidateRefreshTokenAsync(refreshToken);

            if (user == null)
            {
                throw new Exception("Invalid or expired refresh token");
            }

            var newAccessToken = _jwtService.GenerateAccessToken(user.Id, user.Email, user.Role);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            await _authRepository.SaveRefreshTokenAsync(user, newRefreshToken);

            httpContext.Response.Cookies.Append("token", newAccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(5),
                Path = "/"
            });

            httpContext.Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(10)
            });

            return new RefreshTokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }


    }


}

