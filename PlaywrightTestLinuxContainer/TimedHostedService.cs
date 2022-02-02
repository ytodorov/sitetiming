using Microsoft.Playwright;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Microsoft.ApplicationInsights;

namespace PlaywrightTestLinuxContainer
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedHostedService> logger;
        private Timer timer = null!;
        private IBrowser browser;
        private TelemetryClient applicationInsightsClient;

        public TimedHostedService(ILogger<TimedHostedService> logger, IBrowser browser, TelemetryClient applicationInsightsClient)
        {
            this.logger = logger;
            this.browser = browser;
            this.applicationInsightsClient = applicationInsightsClient;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Timed Hosted Service running.");

            timer = new Timer(DoWork, null, 0, Timeout.Infinite);
                


            return Task.CompletedTask;
        }

        private bool IsDoWorkCompleted = true;

        private async void DoWork(object? state)
        {
            //Do not make records in DB when in Mentormate
            //if (HelperMethods.IpAddressOfServer == "217.79.32.194")
            //{
            //    return;
            //}

            applicationInsightsClient.TrackEvent("DoWork Started");

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

                    var randomNumber = (int)HelperMethods.Random.NextInt64(0, countOfAllSites - 1);
                    var randomSite = allSites[randomNumber];

                    var data = await HelperMethods.ExecuteProbeAsync(randomSite.Url, browser);
                    string dataString = data.ToString();



                    //var results = new List<ProbeEntity>();
                    // CPU is 100% on small app service plan in Azure
                    //Parallel.For(0, countOfAllSites, new ParallelOptions() { MaxDegreeOfParallelism = 1 }, (int i) =>
                    //{
                    //    var site = allSites[i];
                    //    var data = HelperMethods.ExecuteProbeAsync(site.Url, browser).Result;
                    //    Thread.Sleep(1000);
                    //    //results.Add(data);
                    //});
                }
                catch (Exception ex)
                {
                    applicationInsightsClient.TrackException(ex);
                    logger.LogError(ex, ex.Message);
                }
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            applicationInsightsClient.TrackEvent("StopAsync Started");
            logger.LogInformation("Timed Hosted Service is stopping.");

            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
