using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.NotificationDto;

namespace Trackify.SubscriptionTracker.Application.Interface
{
    public interface INotificationSender
    {
        Task SendNotificationAsync(int userId, UserNotificationGetDto newNotification);
    }
}
