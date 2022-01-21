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
        public async Task<IActionResult> Get(int take = 10, string? siteUrl = null)
        {
            if (!string.IsNullOrEmpty(siteUrl) && !siteUrl.StartsWith("http"))
            {
                siteUrl = $"http://{siteUrl}";
            }

            var query = SiteTimingContext.Probes.AsQueryable();               

            if (!string.IsNullOrEmpty(siteUrl))
            {
                query = query.Where(s => s.Site.Url == siteUrl);
            }

            query = query.Take(take);
            query = query.Include(s => s.Site);


            var result = await query
                .ToListAsync();

            var jr = new JsonResult(result);
            return jr;
        }

       
    }
}