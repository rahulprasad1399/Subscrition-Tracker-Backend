using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;

namespace Trackify.SubscriptionTracker.Infrastructure.Data.Reposetories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IApplicationDbContext _context;

        public NotificationRepository(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> MarkAsReadAsync(List<int> notificationIds,CancellationToken cancellationToken = default)
        {
            int response =await _context.UserNotifications
                .Where(n=>notificationIds.Contains(n.Id))
                .ExecuteUpdateAsync(setters=>setters
                    .SetProperty(n=>n.IsRead,true)
                    .SetProperty(n=>n.ReadAt,DateTime.UtcNow),
                    cancellationToken);
            return response;
        }
    }
}
