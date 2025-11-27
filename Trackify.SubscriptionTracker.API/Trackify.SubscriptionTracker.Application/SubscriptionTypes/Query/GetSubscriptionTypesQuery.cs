using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Dtos;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.SubscriptionTypes.Query
{
    public class GetSubscriptionTypesQuery : IRequest<IEnumerable<SubscriptionTypeDto>>
    {
    }
    public class GetSubscriptionTypesQueryHandler : IRequestHandler<GetSubscriptionTypesQuery, IEnumerable<SubscriptionTypeDto>>
    {
        private readonly IGenericRepository<SubscriptionType> _repository;

        public GetSubscriptionTypesQueryHandler(IGenericRepository<SubscriptionType> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SubscriptionTypeDto>> Handle(GetSubscriptionTypesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<SubscriptionType> subscriptionTypes = await _repository.GetAllAsync(cancellationToken);
            IEnumerable<SubscriptionTypeDto> subscriptionTypeDtos = subscriptionTypes.Select(x => new SubscriptionTypeDto()
            {
                Id = x.Id,
                TypeName = x.TypeName,
                CreatedByUserId = x.CreatedByUserId,
            });
            return subscriptionTypeDtos;
        }
    }

}
