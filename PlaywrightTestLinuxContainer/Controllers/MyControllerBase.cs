using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlaywrightTestLinuxContainer.Controllers
{
    public class MyControllerBase : ControllerBase
    {
        public SiteTimingContext SiteTimingContext { get; set; }

        public JsonSerializerOptions JsonSerializerOptions { get; set; } = new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.Preserve };


        public MyControllerBase(IServiceProvider serviceProvider)
        {
            SiteTimingContext = serviceProvider.GetRequiredService<SiteTimingContext>();
        }
    }
}
