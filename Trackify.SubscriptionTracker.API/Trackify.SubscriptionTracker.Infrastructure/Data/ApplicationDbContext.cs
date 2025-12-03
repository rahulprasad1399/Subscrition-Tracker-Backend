using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<SubscriptionType> SubscriptionTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Subscription>()
                .Property(s => s.Cost).HasPrecision(18, 2);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Email).HasMaxLength(100);

            modelBuilder.Entity<Service>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId);

            modelBuilder.Entity<SubscriptionType>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId);

            // Subscription → User (cascade)
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserNotification → User (NO CASCADE)
            modelBuilder.Entity<UserNotification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict); 

            // UserNotification → Subscription (cascade)
            modelBuilder.Entity<UserNotification>()
                .HasOne(n => n.Subscription)
                .WithMany()
                .HasForeignKey(n => n.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
