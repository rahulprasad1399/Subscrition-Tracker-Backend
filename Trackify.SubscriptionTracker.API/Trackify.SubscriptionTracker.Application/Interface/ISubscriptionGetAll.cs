using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.Interface
{
    public interface ISubscriptionGetAll
    {
        Task<List<Subscription>> SubscriptionGetAllAsync(int UserId);
        Task<Subscription> SubscriptionGetByIdAsync(int id);
    }
}
