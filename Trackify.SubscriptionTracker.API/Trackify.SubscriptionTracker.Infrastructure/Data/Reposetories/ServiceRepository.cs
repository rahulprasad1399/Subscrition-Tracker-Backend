using Microsoft.EntityFrameworkCore;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Infrastructure.Data.Reposetories
{
    public class ServiceRepository : IServiceGetAllAsync
    {
        private readonly IApplicationDbContext _context;
        public ServiceRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Service>> GetAllServiceAsync(int userId)
        {
            List<Service> services = await _context.Services.Include(c=>c.Category)
                .Where((service)=>service.CreatedByUserId == userId ||
                service.CreatedByUserId == null).ToListAsync();
            
            return services;
        
        }
    }
}
