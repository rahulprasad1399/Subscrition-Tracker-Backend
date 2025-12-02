using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Application.Users.Dto;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.Users.Query
{
    public class ValidateUserQuery : IRequest<GetUserDto>
    {
        public int UserId { get; set; }
    }

    public class ValidateUserQueryHandler : IRequestHandler<ValidateUserQuery, GetUserDto>
    {
        private readonly IGenericRepository<User> _repository;
        public ValidateUserQueryHandler(IGenericRepository<User> repository)
        {
            _repository = repository;
        }
        public async Task<GetUserDto> Handle(ValidateUserQuery request, CancellationToken cancellationToken)
        {
            User existingUser = await _repository.GetByIdAsync(request.UserId);

            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            return new GetUserDto
            {
                FullName = existingUser.FullName,
                Email = existingUser.Email,
                Id = existingUser.Id
            };

        }
    }
}
