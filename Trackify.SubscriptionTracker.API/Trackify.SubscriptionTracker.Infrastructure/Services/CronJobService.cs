using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;

namespace Trackify.SubscriptionTracker.Infrastructure.Services
{
    public class CronJobService : ICronJobService
    {
        public Task ExecuteJobAsync()
        {
            throw new NotImplementedException();
        }
    }
}
