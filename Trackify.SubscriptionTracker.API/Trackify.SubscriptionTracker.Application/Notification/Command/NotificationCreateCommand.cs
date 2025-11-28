using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;

namespace Trackify.SubscriptionTracker.Application.Notification.Command
{
    public class NotificationCreateCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public int SubscriptionId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
    }

    public class NotificationCreateCommandHandler : IRequestHandler<NotificationCreateCommand, bool>
    {
        private readonly IGenericRepository<UserNotification> _genericRepository;
        public NotificationCreateCommandHandler(IGenericRepository<UserNotification> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task<bool> Handle(NotificationCreateCommand request, CancellationToken cancellationToken)
        {
            var notification = new UserNotification(request.UserId, request.SubscriptionId,
                request.Title, request.Type);

            await _genericRepository.AddAsync(notification);

            return true;
        }
    }
}
