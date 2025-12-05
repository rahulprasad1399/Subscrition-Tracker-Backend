using Microsoft.AspNetCore.SignalR;
using Trackify.SubscriptionTracker.APIHub;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Application.NotificationDto;

namespace Trackify.SubscriptionTracker.Infrastructure.Services
{
    public class NotficationHubService : INotificationSender
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotficationHubService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public Task SendNotificationAsync(int userId,UserNotificationGetDto newNotification)
        {
            return _hubContext.Clients.User(userId.ToString())
                .SendAsync("ReceiveNotification", new { newNotification });
        }
    }
}
