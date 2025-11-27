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

namespace Trackify.SubscriptionTracker.Application.SubscriptionTypes.Command
{
    public class DeleteSubscriptionTypeCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class DeleteSubscriptionTypeCommandHandler : IRequestHandler<DeleteSubscriptionTypeCommand, int>
    {
        private readonly IGenericRepository<SubscriptionType> _repository;

        public DeleteSubscriptionTypeCommandHandler(IGenericRepository<SubscriptionType> repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(DeleteSubscriptionTypeCommand request, CancellationToken cancellationToken)
        {
            int response = await _repository.DeleteAsync(request.Id);
            if (response == 0) 
            {
                throw new NotFoundException(nameof(SubscriptionType),request.Id);
            }
            return response;
        }
    }
    public class DeleteSubscriptionTypeCommandValidator : AbstractValidator<DeleteSubscriptionTypeCommand>
    {
        private readonly IGenericRepository<SubscriptionType> _repository;

        public DeleteSubscriptionTypeCommandValidator(IGenericRepository<SubscriptionType> repository)
        {
            _repository = repository;

            RuleFor(x=>x.Id)
                .GreaterThan(0).WithMessage("Subscrption type id is required")
                .MustAsync(async (subscriptionId, cancellation) =>
                {
                    return await _repository.ExistsAsync(subscriptionId, cancellation);
                }).WithMessage("Subscription Type doesn't exist");
        }

    }
}
