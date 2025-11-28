
namespace Trackify.SubscriptionTracker.API
{
    public class CroneJob : IHostedService, IDisposable
    {
        private Timer _timer;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var duetime = TimeSpan.FromSeconds(0);
            var periodTime = TimeSpan.FromSeconds(5);
            _timer = new Timer(DoWork, null, duetime, periodTime);
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Console.WriteLine("Do Work Started");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, 0);
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
