using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Dtos;
using Trackify.SubscriptionTracker.Application.Exceptions;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.Services.Query
{
    public class GetServiceByIdQuery : IRequest<ServiceDto>
    {
        public int Id { get; set; }

    }
    public class GetServiceByIdQueryHAndler : IRequestHandler<GetServiceByIdQuery, ServiceDto>
    {
        private readonly IGenericRepository<Service> _repository;

        public GetServiceByIdQueryHAndler(IGenericRepository<Service> repository)
        {
            _repository = repository;
        }

        public async Task<ServiceDto> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
        {
            Service service = await _repository.GetByIdAsync(request.Id,cancellationToken);
            if (service == null) 
            {
                throw new NotFoundException(nameof(service),request.Id);
            }
            ServiceDto serviceDto = new()
            {
                Id = service.Id,
                ServiceName = service.ServiceName,
                CategoryId = service.CategoryId,
                CreatedByUserId = service.CreatedByUserId,
            };
            return serviceDto;
        }
    }
    public class GetServiceByIdQueryValidator : AbstractValidator<GetServiceByIdQuery> 
    {
        public GetServiceByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Service id required");
        }
    }
}
