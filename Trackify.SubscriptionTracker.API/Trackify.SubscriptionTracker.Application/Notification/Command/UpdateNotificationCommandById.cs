using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.NotificationCommand
{
    public class UpdateNotificationCommandById : IRequest<int>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }

    public class UpdateNotificationCommandHandlerById : IRequestHandler<UpdateNotificationCommandById, int>
    {
        private readonly IGenericRepository<UserNotification> _notificationRepository;
        public UpdateNotificationCommandHandlerById(IGenericRepository<UserNotification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<int> Handle(UpdateNotificationCommandById request, CancellationToken cancellationToken)
        {
            UserNotification notification = await _notificationRepository.GetByIdAsync(request.Id);
            if (notification == null)
            {
                throw new KeyNotFoundException("Notification Not Found");
            }

            if (notification.UserId != request.UserId)
            {
                throw new Exception("User is not allowed to update this notification");
            }

            notification.MarkAsRead();
            int result = await _notificationRepository.UpdateAsync(notification);
            return result;
        }
    }
}
