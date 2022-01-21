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

        private IBrowser browser;

        public ProbeController(ILogger<ProbesController> logger, IBrowser browser)
        {
            this.browser = browser;
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
                    result = await HelperMethods.ExecuteProbeAsync(url, browser);
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