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
    public class ProbesController : MyControllerBase
    {
        public ProbesController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        [HttpGet(Name = "GetProbes")]
        public async Task<IActionResult> Get(int take = int.MaxValue)
        {
            var result = await SiteTimingContext.Probes
                .Take(take)
                .Include(s => s.Site)
                .ToListAsync();

            var jr = new JsonResult(result, JsonSerializerOptions);
            return jr;
        }

       
    }
}