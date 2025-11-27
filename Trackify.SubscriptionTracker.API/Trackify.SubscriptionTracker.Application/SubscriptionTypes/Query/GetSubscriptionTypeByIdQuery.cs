using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Dtos;
using Trackify.SubscriptionTracker.Application.Exceptions;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.SubscriptionTypes.Query
{
    public class GetSubscriptionTypeByIdQuery : IRequest<SubscriptionTypeDto>
    {
        public int Id { get; set; }
    }

    public class GetSubscriptionTypeByIdQueryHandler : IRequestHandler<GetSubscriptionTypeByIdQuery, SubscriptionTypeDto>
    {
        private readonly IGenericRepository<SubscriptionType> _repository;

        public GetSubscriptionTypeByIdQueryHandler(IGenericRepository<SubscriptionType> repository)
        {
            _repository = repository;
        }

        public async Task<SubscriptionTypeDto> Handle(GetSubscriptionTypeByIdQuery request, CancellationToken cancellationToken)
        {
            SubscriptionType subscriptionType = await _repository.GetByIdAsync(request.Id,cancellationToken);
            if (subscriptionType == null)
                throw new NotFoundException(nameof(SubscriptionType), request.Id);
            SubscriptionTypeDto subscriptionTypeDto = new()
            {
                Id = subscriptionType.Id,
                TypeName = subscriptionType.TypeName,
                CreatedByUserId = subscriptionType.CreatedByUserId,
            };
            return subscriptionTypeDto;
        }
    }

    public class GetSubscriptionTypeByIdQueryValidator : AbstractValidator<GetSubscriptionTypeByIdQuery>
    {
        public GetSubscriptionTypeByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Subscription type id is required");
        }
    }
}
