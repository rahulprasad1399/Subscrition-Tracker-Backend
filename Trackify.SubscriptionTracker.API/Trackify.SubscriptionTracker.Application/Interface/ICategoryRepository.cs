using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Infrastructure.Data.Reposetories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<bool> IsNameUniqueAsync(string categoryName, CancellationToken cancellationToken);
    }
}
