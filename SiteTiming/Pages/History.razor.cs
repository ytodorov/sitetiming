using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PlaywrightTestLinuxContainer;
using PlaywrightTestLinuxContainer.Entities;
using Microsoft.EntityFrameworkCore;

namespace SiteTiming.Pages
{
    public partial class History
    {
        [Parameter]
        public string? UrlToGetData { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}
