using Core.Entities;
using Core.Redis;
using IpInfo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Playwright;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlaywrightTestLinuxContainer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProbeController : MyControllerBase
    {

        private IBrowser browser;

        public ProbeController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.browser = serviceProvider.GetService<IBrowser>();
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