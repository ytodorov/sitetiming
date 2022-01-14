using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PlaywrightTestLinuxContainer;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace SiteTiming.Pages
{
    public partial class History
    {
        [Parameter]
        [SupplyParameterFromQuery(Name = "url")]
        public string? UrlToGetData { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        [Inject]
        public SiteTimingContext SiteTimingContext { get; set; }

        public List<ProbeEntity> Timings { get; set; }

        protected override void OnInitialized()
        {
            var isInitializedProp = JsRuntime.GetType().GetProperties().FirstOrDefault(f => f.Name == "IsInitialized");
            bool isJsInitialized = (bool)isInitializedProp.GetValue(JsRuntime);
            if (isJsInitialized == true)
            {
                Timings = SiteTimingContext.Probes.Include(s => s.Site).Where(s => s.Site.Name == UrlToGetData)
                   .OrderByDescending(s => s.DateCreated)
                   .ToList();
            }

            base.OnInitialized();
        }
    }
}
