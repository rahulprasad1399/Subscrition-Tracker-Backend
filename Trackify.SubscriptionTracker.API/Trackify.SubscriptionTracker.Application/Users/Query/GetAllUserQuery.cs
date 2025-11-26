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
    public class GetAllUserQuery : IRequest<List<GetUserDto>>
    {

    }

    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, List<GetUserDto>>
    {
        private readonly IGenericRepository<User> _repository;
        public GetAllUserQueryHandler(IGenericRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetUserDto>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var userList = await _repository.GetAllAsync();
            var users = userList.ToList();

            return users.Select((u) => new GetUserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email
            }).ToList();
        }
    }
}
