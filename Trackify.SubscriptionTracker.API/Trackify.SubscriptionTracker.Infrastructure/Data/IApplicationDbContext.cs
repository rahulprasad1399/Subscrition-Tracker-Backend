using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Infrastructure.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Service> Services { get; set; }
        DbSet<SubscriptionType> SubscriptionTypes { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
