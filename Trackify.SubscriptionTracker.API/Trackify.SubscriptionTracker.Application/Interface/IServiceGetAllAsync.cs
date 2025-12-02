using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackify.SubscriptionTracker.Application.Interface
{
    public interface IServiceGetAllAsync
    {
        Task<List<Trackify.SubscriptionTracker.Domain.Entity.Service>> GetAllServiceAsync(int userId);
    }
}
