using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbSet<User> Users { get; set; }
        DbSet<Subscription> Subscriptions { get; set; }

        DbSet<UserNotification> UserNotifications { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry<T> Entry<T>(T entity) where T : class;
    }
}
