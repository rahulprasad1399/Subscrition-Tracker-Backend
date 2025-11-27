using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Application.Users.Dto;

namespace Trackify.SubscriptionTracker.Application.Users.Command
{
    public class LoginUserCommand : IRequest<LoginResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResponse>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _contextAccessor;
        public LoginUserCommandHandler(IAuthRepository authRepository, IJwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _authRepository = authRepository;
            _jwtService = jwtService;
            _contextAccessor = httpContextAccessor;
        }
        public async Task<LoginResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _authRepository.GetByEmailAsync(request.Email);
            if (!_authRepository.VerifyPassword(user, request.Password))
            {
                throw new Exception("Invalid Login Credentials");
            }

            var token = _jwtService.GenerateAccessToken(user.Id, user.Email, user.Role);
            var refreshToken = _jwtService.GenerateRefreshToken();

            var httpContext = _contextAccessor.HttpContext;
            if (httpContext != null)
            {
                httpContext.Response.Cookies.Append("token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    Path = "/"
                });

                httpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMinutes(10)
                });
            }

            await _authRepository.SaveRefreshTokenAsync(user, refreshToken);

            return new LoginResponse
            {
                FullName = user.FullName,
                Email = user.Email
            };

        }
    }

    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator(IAuthRepository authRepository)
        {
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is Required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is Required")
                .EmailAddress().WithMessage("Invalid Email Format")
                .MustAsync(async (email, cancellation) => await authRepository.CheckEmailExistAsync(email))
                .WithMessage("Invalid Login Credentials");
        }
    }
}
