using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.SubscriptionTypes.Query
{
    public class GetSubscriptionTypesQuery : IRequest<IEnumerable<SubscriptionType>>
    {
    }
    public class GetSubscriptionTypesQueryHandler : IRequestHandler<GetSubscriptionTypesQuery, IEnumerable<SubscriptionType>>
    {
        private readonly IGenericRepository<SubscriptionType> _repository;

        public GetSubscriptionTypesQueryHandler(IGenericRepository<SubscriptionType> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SubscriptionType>> Handle(GetSubscriptionTypesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    } 

}
