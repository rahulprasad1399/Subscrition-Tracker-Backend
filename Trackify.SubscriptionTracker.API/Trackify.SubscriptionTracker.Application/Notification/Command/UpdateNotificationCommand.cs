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
        private readonly INotificationRepository _repository;

        public UpdateNotificationCommandHandler(INotificationRepository repository)
        {
            _repository = repository;
        }
        public async Task<int> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
        {
            List<int> notificationIds = request.NotificationIds;

            int resposonse = await _repository.MarkAsReadAsync(notificationIds,cancellationToken);
            
            return resposonse;
        }
    }
}
