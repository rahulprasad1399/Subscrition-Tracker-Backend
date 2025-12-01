using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Infrastructure.Data;

namespace Trackify.SubscriptionTracker.Application.Notification.Query
{
    public class GetNewNotificationCount : IRequest<int>
    {
        public int UserId { get; set; }
    }

    public class GetNewNotificationCountHandler : IRequestHandler<GetNewNotificationCount, int>
    {
        private readonly IApplicationDbContext _context;
        public GetNewNotificationCountHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(GetNewNotificationCount request, CancellationToken cancellationToken)
        {
            var newUserNotificationCount = await _context.UserNotifications
                .Where((notification) => notification.UserId == request.UserId && notification.IsRead == false)
                .CountAsync();

            return newUserNotificationCount;
        }
    }
}
