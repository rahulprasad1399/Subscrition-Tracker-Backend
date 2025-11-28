using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackify.SubscriptionTracker.Application.Interface
{
    public interface INotificationRepository
    {
        Task<int> MarkAsReadAsync(List<int> notificationIds, CancellationToken cancellationToken = default);
    }
}
