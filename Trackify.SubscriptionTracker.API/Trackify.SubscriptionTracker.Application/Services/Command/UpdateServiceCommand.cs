using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Exceptions;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.Services.Command
{
    public class UpdateServiceCommand : IRequest<int>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public int CategoryId { get; set; }
        [JsonIgnore]
        public int CreatedByUserId { get; set; }
    }
    public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, int>
    {
        private readonly IGenericRepository<Service> _repository;

        public UpdateServiceCommandHandler(IGenericRepository<Service> repository)
        {
            _repository = repository;
        }
        public async Task<int> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
        {
            Service service =await _repository.GetByIdAsync(request.Id,cancellationToken);
            if (service == null) 
            {
                throw new NotFoundException(nameof(Service),request.Id);
            }
            if (service.CreatedByUserId != request.CreatedByUserId) 
            {
                throw new ForbiddenAccessException("You are not authorized to modify this service.");
            }
            service.UpdateService(request.ServiceName, request.CategoryId);
            return await _repository.SaveChangesAsync(cancellationToken);

        }
    }
    public class UpdateServiceCommandValidator : AbstractValidator<UpdateServiceCommand>
    {
        private readonly IGenericRepository<Category> _repository;

        public UpdateServiceCommandValidator(IGenericRepository<Category> repository)
        {
            _repository = repository;
            RuleFor(x => x.ServiceName)
                .NotEmpty().WithMessage("Service name is required")
                .MaximumLength(25).WithMessage("Service name must be less than 25 characters");
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category Id is required")
                .MustAsync(async (categoryId, cancellation) =>
                {
                    return await _repository.ExistsAsync(categoryId, cancellation);
                }).WithMessage("Category doesn't exist");
            RuleFor(x => x.CreatedByUserId)
                .GreaterThan(0).WithMessage("User Id is required");
        }
    }
}
