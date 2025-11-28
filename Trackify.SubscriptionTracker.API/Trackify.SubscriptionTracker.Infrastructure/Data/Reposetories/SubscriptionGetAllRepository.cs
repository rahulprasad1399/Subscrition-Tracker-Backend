using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Infrastructure.Data.Reposetories
{
    public class SubscriptionGetAllRepository : ISubscriptionGetAll
    {
        private readonly IApplicationDbContext _context;
        public SubscriptionGetAllRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Subscription>> SubscriptionGetAllAsync(int UserId)
        {
            return await _context.Subscriptions.Where((sub) => sub.UserId == UserId).Include(x => x.Service)
                        .ThenInclude(x => x.Category)
                        .Include(x => x.SubscriptionType).ToListAsync();
        }

        public async Task<Subscription> SubscriptionGetByIdAsync(int id)
        {
            return await _context.Subscriptions
                .Include(x => x.Service)
                    .ThenInclude(x => x.Category)
                .Include(x => x.SubscriptionType)
                .FirstOrDefaultAsync(sub => sub.Id == id);
        }
    }
}
