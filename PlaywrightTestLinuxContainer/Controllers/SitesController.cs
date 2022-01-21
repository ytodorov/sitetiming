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
    public class SitesController : MyControllerBase
    {

        public SitesController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        [HttpGet(Name = "GetSites")]
        public async Task<IActionResult> Get(int take = int.MaxValue, string? url = null)
        {
            var query = SiteTimingContext.Sites
                .Include(s => s.Probes)
                .OrderByDescending(s => s.Id)
                .Take(take);

            if (!string.IsNullOrEmpty(url) && !url.StartsWith("http"))
            {
                url = $"http://{url}";
            }

            if (!string.IsNullOrEmpty(url))
            {
                query = query.Where(s => s.Url == url);
            }

            var result = await query
                .ToListAsync();

            var jr = new JsonResult(result, JsonSerializerOptions);
            return jr;
        }

    }
}