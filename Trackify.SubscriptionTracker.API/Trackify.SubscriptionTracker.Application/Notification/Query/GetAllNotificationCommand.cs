using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Application.NotificationDto;
using Trackify.SubscriptionTracker.Domain.Entity;
using Trackify.SubscriptionTracker.Infrastructure.Data;

namespace Trackify.SubscriptionTracker.Application.NotificationQuery
{
    public class GetAllNotificationsQuery : IRequest<List<UserNotificationGetDto>>
    {
        public int UserId { get; set; }
    }

    public class GetAllNotificationsQueryHandler
        : IRequestHandler<GetAllNotificationsQuery, List<UserNotificationGetDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllNotificationsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserNotificationGetDto>> Handle(
            GetAllNotificationsQuery request,
            CancellationToken cancellationToken)
        {
            var allNotifications = _context.UserNotifications.Include(x => x.Subscription)
                .ThenInclude(x => x.Service).ThenInclude(x => x.Category)
                .Where((notification) => notification.UserId == request.UserId).OrderByDescending(n => n.CreatedAt);

            var userNotifications = await allNotifications.ToListAsync();

            return userNotifications.Select(n => new UserNotificationGetDto
            {
                Id = n.Id,
                SubscriptionId = n.SubscriptionId,
                Title = n.Title,
                Type = n.Type,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt,
                UserId = n.UserId,
                Cost = n.Subscription.Cost,
                PurchaseDate = n.Subscription.PurchaseDate,
                RenewalDate = n.Subscription.RenewalDate,
                ServiceName = n.Subscription.Service.ServiceName,
                CategoryName = n.Subscription.Service.Category.CategoryName,
                serviceId = n.Subscription.Service.Id
            }).ToList();
        }
    }
}
