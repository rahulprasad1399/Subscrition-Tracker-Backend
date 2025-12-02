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
        public int userId { get; set; }
    }
    public class GetServicesQueryHandler : IRequestHandler<GetServicesQuery, IEnumerable<ServiceDto>>
    {
        private readonly IServiceGetAllAsync _servicerepo;

        public GetServicesQueryHandler(IServiceGetAllAsync servicerepo)
        {
            _servicerepo = servicerepo;
        }

        public async Task<IEnumerable<ServiceDto>> Handle(GetServicesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Service> services = await _servicerepo.GetAllServiceAsync(request.userId);
            IEnumerable<ServiceDto> serviceDtos = services.Select(x => new ServiceDto()
            {
                Id = x.Id,
                ServiceName = x.ServiceName,
                CategoryId = x.CategoryId,
                CreatedByUserId = x.CreatedByUserId,
                CategoryName = x.Category.CategoryName
            });
            return serviceDtos;
        }
    }
}
