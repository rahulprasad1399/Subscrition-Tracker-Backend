using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackify.SubscriptionTracker.Infrastructure.Data.Reposetories
{
    public interface ICatergoryRepository
    {
        Task<bool> AnyAsync(string categoryName);
    }
}
