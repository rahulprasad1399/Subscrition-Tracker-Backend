using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Application.SubscriptionDto;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.SubscriptionQuery
{
    public class SubscriptionGetByIdQuery : IRequest<SubscriptionGetDto>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }

    public class SubscriptionGetByIdQueryHandler : IRequestHandler<SubscriptionGetByIdQuery, SubscriptionGetDto>
    {
        private readonly IGenericRepository<Subscription> _genericRepository;
        public SubscriptionGetByIdQueryHandler(IGenericRepository<Subscription> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task<SubscriptionGetDto> Handle(SubscriptionGetByIdQuery request, CancellationToken cancellationToken)
        {
            Subscription existingSubscription = await _genericRepository.GetByIdAsync(request.Id);
            if (existingSubscription == null)
            {
                throw new KeyNotFoundException("Subscription dosent exist with the given Id");
            }

            return new SubscriptionGetDto
            {
                Id = existingSubscription.Id,
                UserId = existingSubscription.UserId,
                ServiceId = existingSubscription.ServiceId,
                SubscriptionTypeId = existingSubscription.SubscriptionTypeId,
                Cost = existingSubscription.Cost,
                BillingPeriodUnit = existingSubscription.BillingPeriodUnit,
                BillingFrequency = existingSubscription.BillingFrequency,
                PurchaseDate = existingSubscription.PurchaseDate,
                RenewalDate = existingSubscription.RenewalDate,
                Status = existingSubscription.Status

            };
        }
    }
}
