using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Playwright;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlaywrightTestLinuxContainer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProbeController : ControllerBase
    {
        private SiteTimingContext timingContext;

        private readonly ILogger<TimingController> _logger;

        public ProbeController(ILogger<TimingController> logger, SiteTimingContext timingContext)
        {
            _logger = logger;
            this.timingContext = timingContext;
        }

        [HttpGet(Name = "GetProbe")]
        public async Task<ProbeEntity> Get(string? url = null)
        {
            ProbeEntity result = new ProbeEntity();
            try
            {
                if (!string.IsNullOrEmpty(url) && !url.StartsWith("http"))
                {
                    url = $"http://{url}";
                }
                
                if (url != null)
                {
                    result = await HelperMethods.ExecuteProbeAsync(url);
                    result.Site = null;
                    result.Requests = null;
                }
            }
            catch (Exception ex)
            {
                result.ExceptionMessage = ex.Message;
                result.ExceptionStackTrace = ex.StackTrace;
            }

            return result;
        }

    }
}