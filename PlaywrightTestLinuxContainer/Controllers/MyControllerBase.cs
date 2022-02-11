using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlaywrightTestLinuxContainer.Controllers
{
    public class MyControllerBase : ControllerBase
    {
        public SiteTimingContext SiteTimingContext { get; set; }

        //public JsonSerializerOptions JsonSerializerOptions { get; set; } =
        //    new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.Preserve, PropertyNameCaseInsensitive = true };


        //public JsonSerializerSettings JsonSerializerSettings { get; set; } =
        //   new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Serialize };

        public IServiceProvider ServiceProvider { get; set; }

        public MyControllerBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            SiteTimingContext = serviceProvider.GetRequiredService<SiteTimingContext>();
        }
    }
}
