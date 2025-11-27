using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Application.NotificationDto;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.NotificationQuery
{
    public class GetAllNotificationsQuery : IRequest<List<UserNotificationGetDto>>
    {
        public int UserId { get; set; }
    }

    public class GetAllNotificationsQueryHandler
        : IRequestHandler<GetAllNotificationsQuery, List<UserNotificationGetDto>>
    {
        private readonly IGenericRepository<UserNotification> _genericRepository;

        public GetAllNotificationsQueryHandler(IGenericRepository<UserNotification> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<List<UserNotificationGetDto>> Handle(
            GetAllNotificationsQuery request,
            CancellationToken cancellationToken)
        {
            var allNotifications = await _genericRepository.GetAllAsync(cancellationToken);

            var userNotifications = allNotifications
                .Where(n => n.UserId == request.UserId)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();

            return userNotifications.Select(n => new UserNotificationGetDto
            {
                Id = n.Id,
                SubscriptionId = n.SubscriptionId,
                Title = n.Title,
                Type = n.Type,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt,
                ReadAt = n.ReadAt
            }).ToList();
        }
    }
}
