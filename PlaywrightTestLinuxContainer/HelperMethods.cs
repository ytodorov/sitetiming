using AutoMapper;
using Core;
using Core.Classes;
using Core.Entities;
using Core.Redis;
using IpInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Playwright;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Diagnostics;
using System.Text;
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

            url = url.ToLowerInvariant();

            using var transaction = siteTimingContext.Database.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);
            SiteEntity site = null;

            try
            {
                site = await siteTimingContext.Sites.FirstOrDefaultAsync(s => s.Url == url);
                if (site == null)
                {
                    site = new SiteEntity();
                    site.Url = url;
                    await siteTimingContext.Sites.AddAsync(site);
                    await siteTimingContext.SaveChangesAsync();
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }


            ProbeEntity? probe = new ProbeEntity();
            Stopwatch sw = Stopwatch.StartNew();

            // FROM US - https://www.sitetiming.com/http://noi.bg timeout
            float timeout = (float)TimeSpan.FromSeconds(60).TotalMilliseconds;

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
            List<ConsoleMessageEntity> consoleMessagesToSave = new List<ConsoleMessageEntity>();

            try
            {
                page.SetDefaultNavigationTimeout(timeout);
                page.SetDefaultTimeout(timeout);

                page.Console += (s, e) =>
                {
                    string str = e.ToString();
                    ConsoleMessageEntity consoleMessageEntity = new ConsoleMessageEntity()
                    {
                        Text = e.Text,
                        Type = e.Type,
                        Location = e.Location
                    };
                    consoleMessagesToSave.Add(consoleMessageEntity);
                };

                page.RequestFinished += (obj, request) =>
                {
                    var requestEntity = Mapper.Map<RequestEntity>(request.Timing);
                    Mapper.Map(request, requestEntity, RequestType, typeof(RequestEntity));
                    requestsToSave.Add(requestEntity);
                };

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

                    await page.ScreenshotAsync(new PageScreenshotOptions() { Quality = 50, Type = ScreenshotType.Jpeg, Path = shortPagePath50 });

                    //await page.ScreenshotAsync(new PageScreenshotOptions() { Quality = 5, Type = ScreenshotType.Jpeg, Path = shortPagePath5 });

                    //await page.ScreenshotAsync(new PageScreenshotOptions() { Quality = 50, Type = ScreenshotType.Jpeg, FullPage = true, Path = fullPagePath });

                    //var dict = new Dictionary<string, string>();
                    //dict.Add("ContentType", "image/jpeg");

                    await BlobStorageHelper.UploadBlob(shortPagePath50, shortPagePath50, "images", new Dictionary<string, string>());
                    //await BlobStorageHelper.UploadBlob(shortPagePath5, shortPagePath5, "images", new Dictionary<string, string>());
                    //await BlobStorageHelper.UploadBlob(fullPagePath, fullPagePath, "images", dict);

                    //using var image = File.OpenRead(shortPagePath);

                    //var base64 = Utils.ConvertImageToBase64(image, "jpeg");

                    //site.ScreenshotBase64 = base64;
                    //siteTimingContext.Update(site);
                    //File.Delete(fullPagePath);
                    File.Delete(shortPagePath50);
                    //File.Delete(shortPagePath5);
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

            var destinationIpAddressInfo = await GetIpInfo(probe.DestinationIpAddress);
            var sourceIpAddressInfo = await GetIpInfo(probe.SourceIpAddress);

            probe.DestinationIpAddressHostname = destinationIpAddressInfo.Hostname;
            probe.DestinationIpAddressCity = destinationIpAddressInfo.City;
            probe.DestinationIpAddressRegion = destinationIpAddressInfo.Region;
            probe.DestinationIpAddressCountry = destinationIpAddressInfo.Country;
            //probe.DestinationIpAddressLoc = destinationIpAddressInfo.Loc;

            probe.DestinationIpAddressLatitude = double.Parse(destinationIpAddressInfo.Loc.Split(",", StringSplitOptions.RemoveEmptyEntries).FirstOrDefault());
            probe.DestinationIpAddressLongitude = double.Parse(destinationIpAddressInfo.Loc.Split(",", StringSplitOptions.RemoveEmptyEntries).Skip(1).FirstOrDefault());

            probe.DestinationIpAddressPostal = destinationIpAddressInfo.Postal;
            probe.DestinationIpAddressTimezone = destinationIpAddressInfo.Timezone;
            probe.DestinationIpAddressOrg = destinationIpAddressInfo.Org;

            probe.SourceIpAddressHostname = sourceIpAddressInfo.Hostname;
            probe.SourceIpAddressCity = sourceIpAddressInfo.City;
            probe.SourceIpAddressRegion = sourceIpAddressInfo.Region;
            probe.SourceIpAddressCountry = sourceIpAddressInfo.Country;

            //probe.SourceIpAddressLoc = sourceIpAddressInfo.Loc;

            probe.SourceIpAddressLatitude = double.Parse(sourceIpAddressInfo.Loc.Split(",", StringSplitOptions.RemoveEmptyEntries).FirstOrDefault());
            probe.SourceIpAddressLongitude = double.Parse(sourceIpAddressInfo.Loc.Split(",", StringSplitOptions.RemoveEmptyEntries).Skip(1).FirstOrDefault());

            probe.SourceIpAddressPostal = sourceIpAddressInfo.Postal;
            probe.SourceIpAddressTimezone = sourceIpAddressInfo.Timezone;
            probe.SourceIpAddressOrg = sourceIpAddressInfo.Org;

            probe.DistanceBetweenIpAddresses = DistanceBetweenInKm(probe.SourceIpAddressLatitude.GetValueOrDefault(),
                probe.SourceIpAddressLongitude.GetValueOrDefault(),
                probe.DestinationIpAddressLatitude.GetValueOrDefault(),
                probe.DestinationIpAddressLongitude.GetValueOrDefault());

            await siteTimingContext.Probes.AddAsync(probe);
            await siteTimingContext.SaveChangesAsync();

            foreach (var r in requestsToSave)
            {
                r.ProbeId = probe.Id;
            }

            foreach (var c in consoleMessagesToSave)
            {
                c.ProbeId = probe.Id;
            }

            await siteTimingContext.Requests.AddRangeAsync(requestsToSave);
            await siteTimingContext.ConsoleMessages.AddRangeAsync(consoleMessagesToSave);

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

        public static string GetSiteMap()
        {
            StringBuilder sb = new StringBuilder();
            using (SiteTimingContext context = new SiteTimingContext())
            {
                var sitesInDatabase = context.Sites
                    .OrderBy(s => s.Id)
                    .Take(1000)
                    .Select(s => s.Url)
                    .ToList()
                    .Distinct()
                    .ToList();



                sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">");

                sb.AppendLine($@"   <url>
        <loc>https://www.sitetiming.com/</loc>
    </url>");
                sb.AppendLine($@"   <url>
        <loc>https://www.sitetiming.com/sites</loc>
    </url>");
                foreach (var site in sitesInDatabase)
                {
                    sb.AppendLine($@"   <url>
        <loc>https://www.sitetiming.com/{site.ToLowerInvariant()}</loc>
    </url>");
                }

                sb.AppendLine("</urlset>");
            }
            var res = sb.ToString();

            return res;
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


        private static async Task<FullResponse> GetIpInfo(string ip)
        {
            try
            {
                string stringIp = ip;
                //string stringIp = "77.70.29.69"; // HttpContext.Connection.RemoteIpAddress.ToString();

                RedisValue ipDetailsAsJson = RedisConnection.Connection.GetDatabase().StringGet(stringIp);

                if (ipDetailsAsJson.IsNullOrEmpty)
                {
                    using HttpClient httpClient = new HttpClient();
                    IpInfoApi api = new IpInfoApi("6a852f28bb9103", httpClient);

                    FullResponse response = await api.GetInformationByIpAsync(stringIp);

                    string responseAsJson = System.Text.Json.JsonSerializer.Serialize(response);

                    RedisConnection.Connection.GetDatabase().StringSet(stringIp, responseAsJson);

                    return response;

                }
                else
                {
                    FullResponse response = System.Text.Json.JsonSerializer.Deserialize<FullResponse>(ipDetailsAsJson);
                    return response;
                }
            }
            catch (Exception ex)
            {
                return new FullResponse() { Ip = ex.Message };
            }
        }

        private static double DistanceBetweenInKm(double lat1, double lon1, double lat2, double lon2)
        {
            var unit = "K"; // Kilometers
            if (lat1 == lat2 && lon1 == lon2)
            {
                return 0;
            }
            else
            {
                var radlat1 = (Math.PI * lat1) / 180;
                var radlat2 = (Math.PI * lat2) / 180;
                var theta = lon1 - lon2;
                var radtheta = (Math.PI * theta) / 180;
                var dist = Math.Sin(radlat1) * Math.Sin(radlat2) + Math.Cos(radlat1) * Math.Cos(radlat2) * Math.Cos(radtheta);
                if (dist > 1)
                {
                    dist = 1;
                }
                dist = Math.Acos(dist);
                dist = (dist * 180) / Math.PI;
                dist = dist * 60 * 1.1515;
                if (unit == "K")
                {
                    dist = dist * 1.609344;
                }
                if (unit == "N")
                {
                    dist = dist * 0.8684;
                }
                var distString = Math.Floor(dist);

                return distString;
            }
        }



    }
}
