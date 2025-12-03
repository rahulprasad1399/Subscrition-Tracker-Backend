using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Application.Users.Dto;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.Users.Query
{
    public class GetByIdUserQuery : IRequest<GetUserDto>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }

    public class GetByIdUserQueryHandler : IRequestHandler<GetByIdUserQuery, GetUserDto>
    {
        private readonly IGenericRepository<User> _repository;
        public GetByIdUserQueryHandler(IGenericRepository<User> repository)
        {
            _repository = repository;
        }
        public async Task<GetUserDto> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
        {
            var existingUser = await _repository.GetByIdAsync(request.Id);

            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            return new GetUserDto
            {
                FullName = existingUser.FullName,
                Email = existingUser.Email,
                Id = existingUser.Id,
                Image = existingUser.Image
            };
        }
    }

    public class GetByIdUserQueryValidator : AbstractValidator<GetByIdUserQuery>
    {
        public GetByIdUserQueryValidator(IGenericRepository<User> repository)
        {
            RuleFor(x => x.Id).NotNull().WithMessage("Id is required")
                .GreaterThan(0).WithMessage("Provide a valid id");

            RuleFor(x => x.Id).MustAsync(async (id, cancellation) =>
            {
                var user = await repository.GetByIdAsync(id);
                return user != null;
            }).WithMessage("User not found");
        }
    }
}
