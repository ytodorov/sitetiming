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

                    var results = new List<ProbeEntity>();

                    Parallel.For(0, countOfAllSites, new ParallelOptions() { MaxDegreeOfParallelism = 4 }, (int i) =>
                    {
                        var site = allSites[i];
                        var data = HelperMethods.ExecuteProbeAsync(site.Url, browser).Result;
                        results.Add(data);
                    });

                    //foreach (var site in allSites)
                    //{
                    //    var probeGuid = await HelperMethods.ExecuteProbeAsync(site.Url);
                    //}


                    //int batchSize = 4;
                    //int numberOfBatches = countOfAllSites / batchSize;

                    //using var playwright = await Playwright.CreateAsync();
                    //await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });

                    //Parallel.ForEach(allSites, new ParallelOptions() { MaxDegreeOfParallelism = 4 }, async site =>
                    //{
                    //    var data = await HelperMethods.GetData($"http://{site.Name}", site, null, siteTimingContext);
                    //});


                    //for (int i = 0; i < numberOfBatches; i++)
                    //{
                    //    var sites = allSites.Skip(i * batchSize).Take(batchSize).ToList();

                    //    List<Task<string>> tasks = new List<Task<string>>();

                    //    foreach (var site in sites)
                    //    {

                    //        _logger.LogInformation($"Start navigation to http://{site.Name}");
                    //        var task = HelperMethods.GetData($"http://{site.Name}", site, browser);
                    //        tasks.Add(task);
                    //    }

                    //    await Task.WhenAll(tasks);

                    //    var results = new List<string>();

                    //    foreach (var task in tasks)
                    //    {
                    //        results.Add(task.Result);
                    //    }

                    //    var strRes = string.Concat(results);
                    //}
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
