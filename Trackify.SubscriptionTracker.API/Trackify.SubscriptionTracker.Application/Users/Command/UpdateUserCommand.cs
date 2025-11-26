using FluentValidation;
using MediatR;
using System.Text.Json.Serialization;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Application.Users.Dto;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.Users.Command
{
    public class UpdateUserCommand : IRequest<UpdateUser>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUser>
    {
        private readonly IGenericRepository<User> _userRepository;

        public UpdateUserCommandHandler(IGenericRepository<User> repository)
        {
            _userRepository = repository;
        }
        public async Task<UpdateUser> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.Id);
            if (existingUser == null) {
                throw new Exception("User not Found");
            }

            existingUser.UpdateUser(request.FullName, request.Email);

            await _userRepository.UpdateAsync(existingUser);

            return new UpdateUser
            {
                FullName = existingUser.FullName,
                Email = existingUser.Email
            };

        }
    }

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator(IAuthRepository authRepository)
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("FullName is Required").MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is Required")
                .EmailAddress().WithMessage("Enter a valid Email")
                .MustAsync(async (command, email, cancellation) =>
                {
                    var existingUser = await authRepository.GetByEmailAsync(email);
                    if (existingUser == null)
                    {
                        return true;
                    }

                    return existingUser.Id == command.Id;
                }).WithMessage("Email already exist");
        }
    }
}
