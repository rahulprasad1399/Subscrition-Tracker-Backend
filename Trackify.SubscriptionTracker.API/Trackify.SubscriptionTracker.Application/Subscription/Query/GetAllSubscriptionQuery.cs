using MediatR;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Application.SubscriptionDto;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.SubscriptionQuery
{
    public class GetAllSubscriptionQuery : IRequest<List<SubscriptionGetDto>>
    {
    }

    public class GetAllSubscriptionQueryHandler : IRequestHandler<GetAllSubscriptionQuery, List<SubscriptionGetDto>>
    {
        private readonly IGenericRepository<Subscription> _genericRepository;

        public GetAllSubscriptionQueryHandler(IGenericRepository<Subscription> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<List<SubscriptionGetDto>> Handle(GetAllSubscriptionQuery request, CancellationToken cancellationToken)
        {
            var subscriptions = await _genericRepository.GetAllAsync(cancellationToken);

            var subscriptionGetDtos = subscriptions.Select(subscription => new SubscriptionGetDto
            {
                Id = subscription.Id,
                UserId = subscription.UserId,
                ServiceId = subscription.ServiceId,
                SubscriptionTypeId = subscription.SubscriptionTypeId,
                Cost = subscription.Cost,
                BillingPeriodUnit = subscription.BillingPeriodUnit,
                BillingFrequency = subscription.BillingFrequency,
                PurchaseDate = subscription.PurchaseDate,
                RenewalDate = subscription.RenewalDate,
                Status = subscription.Status
            }).ToList();

            return subscriptionGetDtos;
        }
    }
}