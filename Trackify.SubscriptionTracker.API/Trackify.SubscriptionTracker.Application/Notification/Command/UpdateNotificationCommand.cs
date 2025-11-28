using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.NotificationCommand
{
    public class UpdateNotificationCommand : IRequest<int>
    {
        public List<int> NotificationIds { get; set; }
    }

    public class UpdateNotificationCommandHandler : IRequestHandler<UpdateNotificationCommand, int>
    {
        private readonly IGenericRepository<UserNotification> _genericRepository;
        public UpdateNotificationCommandHandler(IGenericRepository<UserNotification> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task<int> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
        {
            int count = 0;
            foreach(int id in request.NotificationIds)
            {
                var notification = await _genericRepository.GetByIdAsync(id);
                if(notification == null)
                {
                    continue;
                }

                notification.MarkAsRead();
                count++;

            }

            await _genericRepository.SaveChangesAsync();
            return count;
        }
    }
}
