using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackify.SubscriptionTracker.Infrastructure.Services
{
    public class CronHostedService : IHostedService
    {
        private Timer _timer;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var dueTime = TimeSpan.FromSeconds(0);
            var periodTime = TimeSpan.FromSeconds(5);

            _timer = new Timer()
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
