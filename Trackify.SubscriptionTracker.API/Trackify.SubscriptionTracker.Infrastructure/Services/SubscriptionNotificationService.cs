using MediatR;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Application.Notification.Command;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Infrastructure.Services
{
    public class SubscriptionNotificationService : ISubscriptionNotificationService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMediator _mediator;
        private readonly IGenericRepository<Subscription> _genericRepository;
        public SubscriptionNotificationService(ISubscriptionRepository subscriptionRepository,
            IMediator mediator, IGenericRepository<Subscription> genericRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _mediator = mediator;
            _genericRepository = genericRepository;
        }
        public async Task CheckUpcomingRenewalsAsync()
        {
            var upcomingSubscriptions = await _subscriptionRepository.GetSubscriptionsForRenewalAsync();
            int respo = await _subscriptionRepository.UpdateSubscriptionsAsync();
            foreach (var subscription in upcomingSubscriptions)
            {
                await _mediator.Send(new NotificationCreateCommand
                {
                    UserId = subscription.UserId,
                    UserEmail = subscription.User.Email,
                    UserName  = subscription.User.FullName,
                    Cost = subscription.Cost,
                    ServiceName = subscription.Service.ServiceName,
                    RenewalDate = subscription.RenewalDate,
                    SubscriptionId = subscription.Id,
                    Title = $"Your subscription for {subscription.Service.ServiceName} is renewing soon",
                    Type = "RenewalReminder"
                });

                subscription.updateSubscriptionNotification();

                await _genericRepository.UpdateAsync(subscription);
            }

        }
    }
}
