using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Dtos;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.Services.Query
{
    public class GetServicesQuery : IRequest<IEnumerable<ServiceDto>>
    {
    }
    public class GetServicesQueryHandler : IRequestHandler<GetServicesQuery, IEnumerable<ServiceDto>> 
    {
        private readonly IGenericRepository<Service> _repository;

        public GetServicesQueryHandler(IGenericRepository<Service> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ServiceDto>> Handle(GetServicesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Service> services = await _repository.GetAllAsync(cancellationToken);
            IEnumerable<ServiceDto> serviceDtos = services.Select(x => new ServiceDto()
            {
                Id = x.Id,
                ServiceName = x.ServiceName,
                CategoryId = x.CategoryId,
                CreatedByUserId = x.CreatedByUserId,
            });
            return serviceDtos;
        }
    }
}
