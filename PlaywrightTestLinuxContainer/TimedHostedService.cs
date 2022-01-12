using Microsoft.Playwright;
using Microsoft.EntityFrameworkCore;

namespace PlaywrightTestLinuxContainer
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer = null!;

        public TimedHostedService(ILogger<TimedHostedService> logger)
        {
            _logger = logger;
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

                    int countOfAllSites = 1000;
                    var allSites = timingContext.Sites.ToList().Take(countOfAllSites).ToList();

                    int batchSize = 1;
                    int numberOfBatches = countOfAllSites / batchSize;

                    using var playwright = await Playwright.CreateAsync();
                    await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });

                    using SiteTimingContext siteTimingContext = new SiteTimingContext();
                    for (int i = 0; i < numberOfBatches; i++)
                    {
                        var sites = allSites.Skip(i * batchSize).Take(batchSize).ToList();

                        List<Task<string>> tasks = new List<Task<string>>();

                        foreach (var site in sites)
                        {

                            _logger.LogInformation($"Start navigation to http://{site.Name}");
                            var task = HelperMethods.GetData($"http://{site.Name}", site, browser, siteTimingContext);
                            tasks.Add(task);
                        }

                        await Task.WhenAll(tasks);

                        var results = new List<string>();

                        foreach (var task in tasks)
                        {
                            results.Add(task.Result);
                        }

                        var strRes = string.Concat(results);
                    }
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
