using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            
        }
    }
}


//public string PasswordHash { get; private set; }
//[Required, MaxLength(25)]
//public string Role { get; private set; }
//public string? RefreshToken { get; private set; }
//public DateTime? RefreshTokenExpiryTime { get; private set; }
//public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
