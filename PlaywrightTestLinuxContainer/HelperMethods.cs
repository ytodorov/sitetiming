using AutoMapper;
using Core;
using Core.Entities;
using Microsoft.Playwright;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlaywrightTestLinuxContainer
{
    public static class HelperMethods
    {
        private static IMapper? Mapper { get; set; }

        private static Type RequestType { get; set; }

        static HelperMethods()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RequestTimingResult, RequestEntity>().ReverseMap();
                RequestType = typeof(IRequest).Assembly.GetTypes().FirstOrDefault(f => f.Name == "Request");
                cfg.CreateMap(RequestType, typeof(RequestEntity));
            });

            Mapper = config.CreateMapper();
        }

        public static string? IpAddressOfServer { get; set; }
        public static async Task<string> GetData(string url, SiteEntity site, IBrowser browser, SiteTimingContext siteTimingContext)
        {
            ProbeEntity? probe = new ProbeEntity();
            Stopwatch sw = Stopwatch.StartNew();
            IPage page = await browser.NewPageAsync();

            List<RequestEntity> requestsToSave = new List<RequestEntity>();

            try
            {
                page.SetDefaultNavigationTimeout(0);
                page.SetDefaultTimeout(0);

                page.RequestFinished += (obj, request) =>
                {
                    var requestEntity = Mapper.Map<RequestEntity>(request.Timing);
                    Mapper.Map(request, requestEntity, RequestType, typeof(RequestEntity));
                    requestsToSave.Add(requestEntity);
                };

                var response = await page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });
                Console.WriteLine($"{sw.ElapsedMilliseconds} finished: var response = await page.GotoAsync(url, new PageGotoOptions {{ WaitUntil = WaitUntilState.DOMContentLoaded }})");

                var res = await page.EvaluateAsync(@"var s = '';
                function f() {
s = JSON.stringify(window.performance.timing)
return s; } f()");

                var stringResult = res.Value.ToString();

                probe = JsonConvert.DeserializeObject<ProbeEntity>(stringResult);

                if (site.ScreenshotBase64 == null)
                {
                    string path = $"{site.Id}.jpeg";
                    await page.ScreenshotAsync(new PageScreenshotOptions() { Quality = 30, Type = ScreenshotType.Jpeg, Path = path });
                    using var image = File.OpenRead(path);

                    var base64 = Utils.ConvertImageToBase64(image, "jpeg");

                    site.ScreenshotBase64 = base64;
                    siteTimingContext.Update(site);
                    File.Delete(path);
                }

                if (site.FaviconBase64 == null)
                {
                    var favIconResult = page.GotoAsync($"https://www.google.com/s2/favicons?domain={url}");

                    if (favIconResult?.Result?.Status == 200)
                    {
                        var body = await favIconResult.Result.BodyAsync();
                        string path = $"{site.Id}FavIcon.png";
                        File.WriteAllBytes(path, body);

                        using var image = File.OpenRead(path);
                        var base64 = Utils.ConvertImageToBase64(image, "png");

                        site.FaviconBase64 = base64;
                        siteTimingContext.Update(site);

                        File.Delete(path);
                    }

                }

                var props = typeof(ProbeEntity).GetProperties();

                var sorted = props.Where(p => p.PropertyType == typeof(long) && (long)p.GetValue(probe) != 0)
                    .OrderBy(p => (long)p.GetValue(probe)).ToList();


                probe.LatencyInChrome = probe.ResponseEnd - probe.FetchStart;
                probe.DOMContentLoadedEventInChrome = probe.DomContentLoadedEventEnd - probe.ConnectStart;
                probe.LoadEventInChrome = probe.LoadEventEnd - probe.ConnectStart;
                probe.IsSuccessfull = true;

                Console.WriteLine($"{sw.ElapsedMilliseconds} finished: timings.IsSuccessfull = true;");
            }
            catch (Exception ex)
            {
                probe.IsSuccessfull = false;
                probe.ExceptionMessage = ex.Message + ex.InnerException?.Message;
                probe.ExceptionStackTrace = ex.StackTrace + ex.InnerException?.StackTrace;
            }
            finally
            {
                await page.CloseAsync();
            }

            probe.SiteId = site.Id;
            probe.SourceIpAddress = IpAddressOfServer;
            probe.TimetakenToGenerateInMs = (long)sw.Elapsed.TotalMilliseconds;
            probe.DateCreated = DateTime.UtcNow;

            

            await siteTimingContext.Probes.AddAsync(probe);
            await siteTimingContext.SaveChangesAsync();

            foreach (var r in requestsToSave)
            {
                r.ProbeId = probe.Id;
                await siteTimingContext.Requests.AddAsync(r);
            }

            try
            {
                await siteTimingContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }




            return probe.ToString();
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
