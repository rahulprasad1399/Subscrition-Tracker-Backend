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
                && sub.Status == Subscription.ActiveStatus.Active).Include(x => x.Service).ToListAsync();
        }
    }
}
