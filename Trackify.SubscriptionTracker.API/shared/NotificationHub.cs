using Microsoft.AspNetCore.SignalR;

namespace Trackify.SubscriptionTracker.APIHub
{
    public class NotificationHub : Hub
    {
        public async Task SendNotificationToUser(string userId, object newNotification)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", newNotification);
        }
    }
}
