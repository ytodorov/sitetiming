using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PlaywrightTestLinuxContainer;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using System.Net;

namespace SiteTiming.Pages
{
    public partial class CatchAllPage
    {
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        [Inject]
        public SiteTimingContext SiteTimingContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        [Parameter]
        //[SupplyParameterFromQuery(Name = "q")]
        public string? UrlToGetData { get; set; }

        public List<ProbeEntity> Probes { get; set; }

        public SiteEntity SiteEntity { get; set; }

        public string ExceptionMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var url = UrlToGetData; // NavigationManager.Uri.Replace(NavigationManager.BaseUri, string.Empty);

                if (PageHelper.IsPageInitializedYordan(JsRuntime, NavigationManager))
                {
                    //string url = UrlToGetData;

                    //url = NavigationManager.Uri.Replace(NavigationManager.BaseUri, string.Empty);
                    if (!url.StartsWith("http"))
                    {
                        url = $"http://{url}";
                    }

                    var response = HttpClient.GetAsync($"http://y-pl.azurewebsites.net/Probe?url={url}").Result;
                    var probeEntity = response.Content.ReadFromJsonAsync<ProbeEntity>(new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true }).Result;

                    Probes = await SiteTimingContext.Probes
                        .Include(s => s.Site)
                        //.Include(s => s.Requests)
                        .Where(s => s.Site.Url == url)
                       .OrderByDescending(s => s.DateCreated)
                       .ToListAsync();

                    SiteEntity = Probes.FirstOrDefault().Site;
                }
            }
            catch (Exception ex)
            {
                ExceptionMessage = ex.Message + ex.StackTrace;
            }

            await base.OnInitializedAsync();
        }
        //protected override void OnInitialized()
        //{
        //    var url = NavigationManager.Uri.Replace(NavigationManager.BaseUri, string.Empty);
           
        //    if (PageHelper.IsPageInitializedYordan(JsRuntime, NavigationManager))
        //    {
        //        //string url = UrlToGetData;

        //        //url = NavigationManager.Uri.Replace(NavigationManager.BaseUri, string.Empty);
        //        if (!url.StartsWith("http"))
        //        {
        //            url = $"http://{url}";
        //        }

        //        var response = HttpClient.GetAsync($"http://y-pl.azurewebsites.net/Probe?url={url}").Result;
        //        var probeEntity = response.Content.ReadFromJsonAsync<ProbeEntity>(new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true }).Result;

        //        Probes = SiteTimingContext.Probes
        //            .Include(s => s.Site)
        //            //.Include(s => s.Requests)
        //            .Where(s => s.Site.Url == url)
        //           .OrderByDescending(s => s.DateCreated)
        //           .ToList();

        //        SiteEntity = Probes.FirstOrDefault().Site;
        //    }

        //    base.OnInitialized();
        //}
    }
}
