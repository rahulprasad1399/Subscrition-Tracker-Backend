using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Exceptions;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.Services.Command
{
    public class DeleteServiceCommand :IRequest<int>
    {
        public int Id { get; set; }
    }
    public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, int>
    {
        private readonly IGenericRepository<Service> _repository;

        public DeleteServiceCommandHandler(IGenericRepository<Service> repository)
        {
            _repository = repository;
        }
        public async Task<int> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
            int response =await _repository.DeleteAsync(request.Id, cancellationToken);
            if (response == 0)
                throw new NotFoundException(nameof(Service), request.Id);
            return response;
        }
    }
    public class DeleteServiceCommandValidator : AbstractValidator<DeleteServiceCommand>
    {
        private readonly IGenericRepository<Service> _repository;

        public DeleteServiceCommandValidator(IGenericRepository<Service> repository)
        {
            _repository = repository;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Service id is required")
                .MustAsync(async (subscriptionId, cancellation) =>
                {
                    return await _repository.ExistsAsync(subscriptionId, cancellation);
                }).WithMessage("Service doesn't exist");
        }
    }
}
