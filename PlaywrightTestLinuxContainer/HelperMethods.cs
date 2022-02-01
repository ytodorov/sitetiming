using AutoMapper;
using Core;
using Core.Classes;
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
        public static Random Random { get; } = new Random();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="browser"></param>
        /// <returns>Probe</returns>
        public static async Task<ProbeEntity> ExecuteProbeAsync(string url, IBrowser browser)
        {

            using SiteTimingContext siteTimingContext = new SiteTimingContext();

            SiteEntity site = siteTimingContext.Sites.FirstOrDefault(s => s.Url == url);
            if (site == null)
            {
                site = new SiteEntity();
                site.Url = url;
                await siteTimingContext.Sites.AddAsync(site);
                await siteTimingContext.SaveChangesAsync();
            }

            ProbeEntity? probe = new ProbeEntity();
            Stopwatch sw = Stopwatch.StartNew();

            float timeout = (float)TimeSpan.FromMinutes(2).TotalMilliseconds;

            //using var playwright = await Playwright.CreateAsync();
            //await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true, Timeout = timeout });

            BrowserNewPageOptions browserNewPageOptionsNew = new BrowserNewPageOptions();
            browserNewPageOptionsNew.IgnoreHTTPSErrors = true;
            browserNewPageOptionsNew.BypassCSP = true;
            
            // detect headless brower problem
            // https://github.com/puppeteer/puppeteer/issues/3656#issuecomment-447111512
            // This fix this problem
            browserNewPageOptionsNew.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.71 Safari/537.36"; //"{ userAgent: 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36' }"
            IPage page = await browser.NewPageAsync(browserNewPageOptionsNew);

            List<RequestEntity> requestsToSave = new List<RequestEntity>();

            try
            {
                page.SetDefaultNavigationTimeout(timeout);
                page.SetDefaultTimeout(timeout);

                //page.RequestFinished += (obj, request) =>
                //{
                //    var requestEntity = Mapper.Map<RequestEntity>(request.Timing);
                //    Mapper.Map(request, requestEntity, RequestType, typeof(RequestEntity));
                //    requestsToSave.Add(requestEntity);
                //};

                // detect headless brower problem
                // https://github.com/puppeteer/puppeteer/issues/3656#issuecomment-447111512

                //page.Request += async (obj, request) =>
                //{
                //    var allHeaders = await request.AllHeadersAsync();
                //    allHeaders.Clear();
                //    request.Headers.Clear();
                //};

                // Do not wait for NetworkIdle because this may never happen - amazon.com
                // Do not wat for Load - https://www.gov.cn/ his may never happen
                // ONLY WAIT FOR DOMContentLoaded


                var response = await page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });

                // Important for navigation error, important for invisible elements: State = WaitForSelectorState.Attached
                var selector = await page.WaitForSelectorAsync("html", new PageWaitForSelectorOptions() { State = WaitForSelectorState.Attached });
                selector = await page.WaitForSelectorAsync("body", new PageWaitForSelectorOptions() { State = WaitForSelectorState.Attached });

                // Just wait a little more to load more images
                //Thread.Sleep(1000);

                //var allHeaders = await response?.AllHeadersAsync();

                var res = await page.EvaluateAsync(@"var s = '';
                function f() {
s = JSON.stringify(window.performance.timing)
return s; } f()");

                var stringResult = res.Value.ToString();

                probe = JsonConvert.DeserializeObject<ProbeEntity>(stringResult);

                if (response != null)
                {
                    var ipaddress = await response.ServerAddrAsync();
                    probe.DestinationIpAddress = ipaddress.IpAddress;
                }

                // Too much data - save only in blob storage

                //if (probe.ScreenshotBase64 == null)
                //{
                //    string path = $"{site.Id}.jpeg";
                //    await page.ScreenshotAsync(new PageScreenshotOptions() { Quality = 30, Type = ScreenshotType.Jpeg, Path = path });
                //    using var image = File.OpenRead(path);

                //    var base64 = Utils.ConvertImageToBase64(image, "jpeg");

                //    probe.ScreenshotBase64 = base64;
                //    //siteTimingContext.Update(site);
                //    File.Delete(path);
                //}

                var title = await page.TitleAsync();
                site.Title = title;

                //if (site.ScreenshotBase64 == null)
                {
                    string shortPagePath50 = $"short50_{probe.UniqueGuid}.jpeg";

                    string shortPagePath5 = $"short5_{probe.UniqueGuid}.jpeg";

                    string fullPagePath = $"full_{probe.UniqueGuid}.jpeg";
                    
                    //await page.ScreenshotAsync(new PageScreenshotOptions() { Quality = 50, Type = ScreenshotType.Jpeg, Path = shortPagePath50 });

                    await page.ScreenshotAsync(new PageScreenshotOptions() { Quality = 5, Type = ScreenshotType.Jpeg, Path = shortPagePath5 });

                    //await page.ScreenshotAsync(new PageScreenshotOptions() { Quality = 50, Type = ScreenshotType.Jpeg, FullPage = true, Path = fullPagePath });

                    //var dict = new Dictionary<string, string>();
                    //dict.Add("ContentType", "image/jpeg");

                    //await BlobStorageHelper.UploadBlob(shortPagePath50, shortPagePath50, "images", dict);
                    await BlobStorageHelper.UploadBlob(shortPagePath5, shortPagePath5, "images", new Dictionary<string, string>());
                    //await BlobStorageHelper.UploadBlob(fullPagePath, fullPagePath, "images", dict);

                    //using var image = File.OpenRead(shortPagePath);

                    //var base64 = Utils.ConvertImageToBase64(image, "jpeg");

                    //site.ScreenshotBase64 = base64;
                    //siteTimingContext.Update(site);
                    //File.Delete(fullPagePath);
                    //File.Delete(shortPagePath50);
                    File.Delete(shortPagePath5);
                }

                /*
                 * var getFavicon = function(){
    var favicon = undefined;
    var nodeList = document.getElementsByTagName("link");
    for (var i = 0; i < nodeList.length; i++)
    {
        if((nodeList[i].getAttribute("rel") == "icon")||(nodeList[i].getAttribute("rel") == "shortcut icon"))
        {
            favicon = nodeList[i].getAttribute("href");
        }
    }
    return favicon;        
}

alert(getFavicon());
                 */
                //if (site.FaviconBase64 == null)
                //{
                //    var favIconResult = page.GotoAsync($"https://www.google.com/s2/favicons?domain={url}");

                //    if (favIconResult?.Result?.Status == 200)
                //    {
                //        var body = await favIconResult.Result.BodyAsync();
                //        string favIconPath = $"favicon_{probe.UniqueGuid}.png";

                //        File.WriteAllBytes(favIconPath, body);

                //        await BlobStorageHelper.UploadBlob(favIconPath, favIconPath, "images", new Dictionary<string, string>());

                //        //using var image = File.OpenRead(favIconPath);
                //        //var base64 = Utils.ConvertImageToBase64(image, "png");

                //        //site.FaviconBase64 = base64;
                //        //siteTimingContext.Update(site);

                //        File.Delete(favIconPath);
                //    }

                //}

                var props = typeof(ProbeEntity).GetProperties();

                var sorted = props.Where(p => p.PropertyType == typeof(long) && (long)p.GetValue(probe) != 0)
                    .OrderBy(p => (long)p.GetValue(probe)).ToList();


                probe.LatencyInChrome = probe.ResponseEnd - probe.FetchStart;
                probe.DOMContentLoadedEventInChrome = probe.DomContentLoadedEventEnd - probe.ConnectStart;
                probe.LoadEventInChrome = probe.LoadEventEnd - probe.ConnectStart;
                probe.IsSuccessfull = true;
            }
            catch (Exception ex)
            {
                var shortmessage = ex.Message;
                var index = ex.Message.IndexOf("=========================== logs ===========================");
                if (index >= 0)
                {
                    shortmessage = ex.Message.Substring(0, ex.Message.IndexOf("=========================== logs ===========================")).Trim();
                }

                //Console.WriteLine("Exception:" + ex.Message);
                probe.IsSuccessfull = false;
                probe.ExceptionMessage = shortmessage;
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
            }

            await siteTimingContext.Requests.AddRangeAsync(requestsToSave);

            try
            {
                await siteTimingContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:" + ex.Message);
            }

            Console.WriteLine($"{sw.ElapsedMilliseconds} ms. taken for {url}");
            return probe;
        }

        public static void PopulateSitesInDatabaseFromFile()
        {
            using (SiteTimingContext context = new SiteTimingContext())
            {
                context.Database.EnsureCreated();
                if (!context.Sites.Any())
                {
                    var sitesInDatabase = context.Sites.Select(s => s.Name).ToList();
                    var sitesInFile = GetSitesFromFile().Take(10000).ToList();
                    var sitesToAdd = new List<SiteEntity>();
                    foreach (var siteInFile in sitesInFile)
                    {
                        if (!sitesInDatabase.Any(s => s.Equals(siteInFile, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            var newSite = new SiteEntity { Name = siteInFile };
                            if (!newSite.Name.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                            {
                                newSite.Url = $"http://{newSite.Name}";
                            }
                            sitesToAdd.Add(newSite);
                            
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
