using Core;
using Microsoft.Playwright;
using Newtonsoft.Json;
using PlaywrightTestLinuxContainer.Entities;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlaywrightTestLinuxContainer
{
    public static class HelperMethods
    {
        public static string? IpAddressOfServer { get; set; }
        public static async Task<string> GetData(string url, SiteEntity site, IBrowser browser, SiteTimingContext siteTimingContext)
        {
            TimingEntity? timings = new TimingEntity();
            Stopwatch sw = Stopwatch.StartNew();
            IPage page = await browser.NewPageAsync();
            try
            {
                page.SetDefaultNavigationTimeout(0);
                page.SetDefaultTimeout(0);

                //page.DOMContentLoaded += (obj, page) =>
                //{
                //    var message = $"Page {page.Url} loaded!";
                //    Console.WriteLine(message);
                //};

                var response = await page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });
                //await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded, new PageWaitForLoadStateOptions() { Timeout = 0 });

                var res = await page.EvaluateAsync(@"var s = '';
                function f() {
s = JSON.stringify(window.performance.timing)
return s; } f()");

                if (site.ScreenshotBase64 == null)
                {
                    string path = $"{site.Id}.jpeg";
                    await page.ScreenshotAsync(new PageScreenshotOptions() { Quality = 20, Type = ScreenshotType.Jpeg, Path = path });
                    using var image = File.OpenRead(path);

                    var base64 = Utils.ConvertImageToBase64(image, "jpeg");

                    site.ScreenshotBase64 = base64;
                    siteTimingContext.Update(site);
                    File.Delete(path);
                }

                var stringResult = res.Value.ToString();

                timings = JsonConvert.DeserializeObject<TimingEntity>(stringResult);

                var props = typeof(TimingEntity).GetProperties();

                var sorted = props.Where(p => p.PropertyType == typeof(long) && (long)p.GetValue(timings) != 0)
                    .OrderBy(p => (long)p.GetValue(timings)).ToList();


                timings.LatencyInChrome = timings.ResponseEnd - timings.FetchStart;
                timings.DOMContentLoadedEventInChrome = timings.DomContentLoadedEventEnd - timings.ConnectStart;
                timings.LoadEventInChrome = timings.LoadEventEnd - timings.ConnectStart;
                timings.IsSuccessfull = true;
            }
            catch (Exception ex)
            {
                timings.IsSuccessfull = false;
                timings.ExceptionMessage = ex.Message + ex.InnerException?.Message;
                timings.ExceptionStackTrace = ex.StackTrace + ex.InnerException?.StackTrace;
            }
            finally
            {
                await page.CloseAsync();
            }

            timings.SiteId = site.Id;
            timings.SourceIpAddress = IpAddressOfServer;
            timings.TimetakenToGenerateInMs = (long)sw.Elapsed.TotalMilliseconds;
            timings.DateCreated = DateTime.UtcNow;

            await siteTimingContext.Timings.AddAsync(timings);
            await siteTimingContext.SaveChangesAsync();


            return timings.ToString();
        }

        public static void PopulateSitesInDatabaseFromFile()
        {
            using (SiteTimingContext context = new SiteTimingContext())
            {
                context.Database.EnsureCreated();
                if (!context.Sites.Any())
                {
                    var sitesInDatabase = context.Sites.Select(s => s.Name).ToList();
                    var sitesInFile = GetSitesFromFile().Take(1000).ToList();
                    var sitesToAdd = new List<SiteEntity>();
                    foreach (var siteInFile in sitesInFile)
                    {
                        if (!sitesInDatabase.Any(s => s.Equals(siteInFile, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            sitesToAdd.Add(new SiteEntity { Name = siteInFile });
                        }
                    }

                    context.Sites.AddRange(sitesToAdd);
                    context.SaveChanges();
                }
            }
        }

        private static List<string> GetSitesFromFile()
        {
            var lines = File.ReadAllLines("top-1m.csv");
            List<string> result = new List<string>();

            foreach (var line in lines)
            {
                var siteName = line.Split(",", StringSplitOptions.RemoveEmptyEntries)[1];
                result.Add(siteName);
            }

            return result;
        }
    }
}
