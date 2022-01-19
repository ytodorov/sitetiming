﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PlaywrightTestLinuxContainer;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using System.Net;

namespace SiteTiming.Pages
{
    public partial class IsItDown
    {
        [Parameter]
        //[SupplyParameterFromQuery(Name = "url")]
        public string? UrlToGetData { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "url")]
        public string? UrlFromQuery { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        [Inject]
        public SiteTimingContext SiteTimingContext { get; set; }

        public List<ProbeEntity> Timings { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //var isInitializedProp = JsRuntime.GetType().GetProperties().FirstOrDefault(f => f.Name == "IsInitialized");
            //bool isJsInitialized = (bool)isInitializedProp.GetValue(JsRuntime);
            //if (isJsInitialized == true)
            //{

            if (UrlToGetData != null && !UrlToGetData.Contains("_blazor/initializers", StringComparison.InvariantCultureIgnoreCase))
            {

                string url = UrlToGetData;
                if (!url.StartsWith("http"))
                {
                    url = $"http://{url}";
                }
                using HttpClient client = new HttpClient() { };

                //HelperMethods.IpAddressOfServer = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();

                var response = await client.GetAsync($"http://y-pl.azurewebsites.net/Probe?url={url}");
                var probeEntity = await response.Content.ReadFromJsonAsync<ProbeEntity>(new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });



                Timings = await SiteTimingContext.Probes
                    .Include(s => s.Site)
                    //.Include(s => s.Requests)
                    .Where(s => s.Site.Url == url)
                   .OrderByDescending(s => s.DateCreated)
                   .ToListAsync();
            }
            //}

            await base.OnInitializedAsync();
        }
        //protected override void OnInitialized()
        //{
        //    var isInitializedProp = JsRuntime.GetType().GetProperties().FirstOrDefault(f => f.Name == "IsInitialized");
        //    bool isJsInitialized = (bool)isInitializedProp.GetValue(JsRuntime);
        //    if (isJsInitialized == true)
        //    {
        //        Timings = SiteTimingContext.Probes
        //            .Include(s => s.Site)
        //            .Include(s => s.Requests)
        //            .Where(s => s.Site.Name == UrlToGetData)
        //           .OrderByDescending(s => s.DateCreated)
        //           .ToList();
        //    }

        //    base.OnInitialized();
        //}
    }
}
