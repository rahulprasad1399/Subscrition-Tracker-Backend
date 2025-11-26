using FluentValidation;
using MediatR;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.Users.Command
{
    public class CreateUserCommand : IRequest<int>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IGenericRepository<User> _genericRepository;
        private readonly IAuthRepository _authRepository;

        public CreateUserCommandHandler(IGenericRepository<User> genericRepository, IAuthRepository authRepository)
        {
            _genericRepository = genericRepository;
            _authRepository = authRepository;
        }
        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

            User user = new User(request.FullName, request.Email, passwordHash: null, role: "Subscriber");
            var hashedPassword = _authRepository.HashPassword(user, request.Password);

            user.SetPaswordHash(hashedPassword);

            await _genericRepository.AddAsync(user);

            return user.Id;

        }
    }

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator(IAuthRepository authRepository)
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("FullName is Required").MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is Required")
                .EmailAddress().WithMessage("Invalid Email Format")
                .MustAsync(async (email, cancellation) => !await authRepository.CheckEmailExistAsync(email)).WithMessage("Email already in use");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is Required");
        }
    }
}

