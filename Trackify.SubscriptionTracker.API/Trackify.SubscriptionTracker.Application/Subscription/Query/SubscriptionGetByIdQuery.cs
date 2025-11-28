using MediatR;
using System.Text.Json.Serialization;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Application.SubscriptionGetByIdDtos;
using Trackify.SubscriptionTracker.Domain.Entity;
using static Trackify.SubscriptionTracker.Domain.Entity.Subscription;

namespace Trackify.SubscriptionTracker.Application.SubscriptionQuery
{
    public class SubscriptionGetByIdQuery : IRequest<SubscriptionGetByIdDto>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }

    public class SubscriptionGetByIdQueryHandler : IRequestHandler<SubscriptionGetByIdQuery, SubscriptionGetByIdDto>
    {
        private readonly ISubscriptionGetAll _subscriptionrepo;
        public SubscriptionGetByIdQueryHandler(ISubscriptionGetAll subscriptionrepo)
        {
            _subscriptionrepo = subscriptionrepo;
        }
        public async Task<SubscriptionGetByIdDto> Handle(SubscriptionGetByIdQuery request, CancellationToken cancellationToken)
        {
            Subscription existingSubscription = await _subscriptionrepo.SubscriptionGetByIdAsync(request.Id);
            if (existingSubscription == null)
            {
                throw new KeyNotFoundException("Subscription dosent exist with the given Id");
            }

            return new SubscriptionGetByIdDto
            {
                Id = existingSubscription.Id,
                ServiceId = existingSubscription.ServiceId,
                ServiceName = existingSubscription.Service.ServiceName,
                subscriptionTypeId = existingSubscription.SubscriptionTypeId,
                SubscriptionTypeName = existingSubscription.SubscriptionType.TypeName,
                categoryId = existingSubscription.Service.CategoryId,
                CategoryName = existingSubscription.Service.Category.CategoryName,
                Cost = existingSubscription.Cost,
                PurchaseDate = existingSubscription.PurchaseDate,
                RenewalDate = existingSubscription.RenewalDate,
                Status = existingSubscription.Status,
                BillingPeriodUnit = existingSubscription.BillingPeriodUnit,
                BillingFrequency = existingSubscription.BillingFrequency,
            };
        }
    }
}
