using Microsoft.Playwright;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace PlaywrightTestLinuxContainer
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer = null!;
        private IBrowser browser;

        public TimedHostedService(ILogger<TimedHostedService> logger, IBrowser browser)
        {
            _logger = logger;
            this.browser = browser;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(10000));


            return Task.CompletedTask;
        }

        private bool IsDoWorkCompleted = true;

        private async void DoWork(object? state)
        {
            // Do not make records in DB when in Mentormate
            //if (HelperMethods.IpAddressOfServer == "217.79.32.194")
            //{
            //    return;
            //}
            while (true)
            {
                try
                {
                    using SiteTimingContext timingContext = new SiteTimingContext();

                    var allSites = timingContext.Sites
                         .OrderBy(s => s.Id)
                        .Select(s => new { s.Url, s.Name })

                        .ToList().ToList();
                    int countOfAllSites = allSites.Count;

                    //var results = new List<ProbeEntity>();
                    // CPU is 100% on small app service plan in Azure
                    Parallel.For(0, countOfAllSites, new ParallelOptions() { MaxDegreeOfParallelism = 1 }, (int i) =>
                    {
                        var site = allSites[i];
                        var data = HelperMethods.ExecuteProbeAsync(site.Url, browser).Result;
                        Thread.Sleep(1000);
                        //results.Add(data);
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
