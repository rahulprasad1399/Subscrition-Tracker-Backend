using MediatR;
using System.ComponentModel.DataAnnotations;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.Users.Command
{
    public class CreateUserCommand : IRequest<int>
    {
        [MaxLength(100)]
        public string FullName { get; set; }
        [EmailAddress, Required, MaxLength(100)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }    
    }

    public class UserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IGenericRepository<User> _genericRepository;
        private readonly IAuthRepository _authRepository;

        public UserCommandHandler(IGenericRepository<User> genericRepository, IAuthRepository authRepository)
        {
            _genericRepository = genericRepository;
            _authRepository = authRepository;
        }
        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if(await _authRepository.CheckEmailExistAsync(request.Email))
            {
                throw new Exception("Email already in use");
            }


            User user = new User(request.FullName, request.Email, passwordHash : null, role : "Subscriber" );
            var hashedPassword = _authRepository.HashPassword(user, request.Password);

            user.SetPaswordHash(hashedPassword);

            await _genericRepository.AddAsync(user);

            return user.Id;

        }
    }
}

