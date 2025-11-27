using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;
using static Trackify.SubscriptionTracker.Domain.Entity.Subscription;

namespace Trackify.SubscriptionTracker.Application.SubscriptionCommand
{
    public class UpdateSubscriptionStatusCommand : IRequest<int>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public ActiveStatus Status { get; set; }
    }

    public class UpdateSubscriptionStatusCommandHandler : IRequestHandler<UpdateSubscriptionStatusCommand, int>
    {
        private readonly IGenericRepository<Subscription> _genericRepository;
        public UpdateSubscriptionStatusCommandHandler(IGenericRepository<Subscription> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task<int> Handle(UpdateSubscriptionStatusCommand request, CancellationToken cancellationToken)
        {
            var existingSubscription = await _genericRepository.GetByIdAsync(request.Id);

            existingSubscription.UpdateStatus(request.Status);

            return await _genericRepository.UpdateAsync(existingSubscription);
        }
    }

    public class UpdateSubscriptionStatusCommandValidator : AbstractValidator<UpdateSubscriptionStatusCommand>
    {
        public UpdateSubscriptionStatusCommandValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid active status");
        }
    }
}
