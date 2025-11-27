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
    public class UpdateSubscriptionCommand : IRequest<int>
    {
        [JsonIgnore]
        public int Id { get; set; } 
        public int ServiceId { get; set; }
        public int SubscriptionTypeId { get; set; }
        public decimal Cost { get; set; }
        public int BillingFrequency { get; set; }
        public string BillingPeriodUnit { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime RenewalDate { get; set; }
        public ActiveStatus Status { get; set; }
    }

    public class UpdateSubscriptionCommandHandler : IRequestHandler<UpdateSubscriptionCommand, int>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Subscription> _subscriptionRepository;
        private readonly IGenericRepository<SubscriptionType> _subscriptionTypeRepository;
        private readonly IGenericRepository<Subscription> _genericRepository;
        public UpdateSubscriptionCommandHandler(IGenericRepository<User> userRepository,
            IGenericRepository<Subscription> subscriptionRepository,
            IGenericRepository<SubscriptionType> subscriptionTypeRepository,
            IGenericRepository<Subscription> genericRepository)
        {
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
            _subscriptionTypeRepository = subscriptionTypeRepository;
            _genericRepository = genericRepository;
        }
        public async Task<int> Handle(UpdateSubscriptionCommand request, CancellationToken cancellationToken)
        {

            if (!await _subscriptionRepository.ExistsAsync(request.ServiceId))
            {
                throw new KeyNotFoundException("Subscription Id not found");
            }

            if (!await _subscriptionTypeRepository.ExistsAsync(request.SubscriptionTypeId))
            {
                throw new KeyNotFoundException("Subscription Type Id not found");
            }

            var existingSubscription = await _genericRepository.GetByIdAsync(request.Id);

            existingSubscription.UpdateSubscription(existingSubscription.ServiceId, existingSubscription.SubscriptionTypeId, 
                existingSubscription.Cost, existingSubscription.BillingFrequency, existingSubscription.BillingPeriodUnit,
                existingSubscription.PurchaseDate, existingSubscription.RenewalDate, existingSubscription.Status);

            return await _genericRepository.UpdateAsync(existingSubscription);
        }
    }

    public class UpdateSubscriptionCommandValidator : AbstractValidator<UpdateSubscriptionCommand>
    {
        public UpdateSubscriptionCommandValidator()
        {
            RuleFor((x) => x.ServiceId)
    .NotNull().WithMessage("Service Id cannot be null");

            RuleFor((x) => x.SubscriptionTypeId)
                .NotNull().WithMessage("SubscriptionType Id cannot be null");

            RuleFor((x) => x.Cost)
                .NotNull().WithMessage("Cost cannot be null")
                .GreaterThanOrEqualTo(0).WithMessage("Cost cannot be negative");

            RuleFor((x) => x.BillingFrequency)
                .NotNull().WithMessage("BillingFrequency cannot be null")
                .GreaterThan(0).WithMessage("BillingFrequency must be greater than 0");

            RuleFor((x) => x.BillingPeriodUnit)
                .NotEmpty().WithMessage("BillingPeriodUnit cannot be null");

            RuleFor(x => x.PurchaseDate)
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Purchase date cannot be in the future.");

            RuleFor(x => x)
                .Must(x => x.RenewalDate > x.PurchaseDate).WithMessage("Renewal date must be greater than purchase date.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid active status");
        }
    }
}
