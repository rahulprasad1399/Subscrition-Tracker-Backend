using Microsoft.EntityFrameworkCore;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Infrastructure.Data.Reposetories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly IApplicationDbContext _context;
        public SubscriptionRepository(IApplicationDbContext contex)
        {
            _context = contex;
        }

        public async Task<List<Subscription>> GetSubscriptionsForRenewalAsync()
        {
            return await _context.Subscriptions.Where((sub) => !sub.IsSendNotification
                && sub.RenewalDate.Date <= DateTime.UtcNow.AddDays(2).Date
                && sub.Status == Subscription.ActiveStatus.Active).Include(x => x.Service)
                .Include(x=>x.User).Include(x=>x.SubscriptionType)
                .ToListAsync();
        }
        public async Task<int> UpdateSubscriptionsAsync()
        {
            // Get all subscriptions whose renewal date has passed and are still Active
            var expiredSubscriptions = await _context.Subscriptions
                .Where(sub => sub.RenewalDate.Date < DateTime.UtcNow.Date
                    && sub.Status == Subscription.ActiveStatus.Active)
                .ToListAsync();

            if (expiredSubscriptions.Count == 0)
                return 0;

            // Update the status to Cancelled
            foreach (var subscription in expiredSubscriptions)
            {
                subscription.UpdateStatus(Subscription.ActiveStatus.Cancelled);
            }

            // Save changes in the database
            return await _context.SaveChangesAsync();
        }

    }
}
