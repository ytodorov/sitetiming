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
    public class TimingController : ControllerBase
    {
        private SiteTimingContext timingContext;

        private readonly ILogger<TimingController> _logger;

        public TimingController(ILogger<TimingController> logger, SiteTimingContext timingContext)
        {
            _logger = logger;
            this.timingContext = timingContext;
        }

        [HttpGet(Name = "GetTimings")]
        public async Task<List<ProbeEntity>> Get(int take = int.MaxValue)
        {
            var timigns = await timingContext.Probes.Take(take).ToListAsync();
            return timigns;
        }

       
    }
}