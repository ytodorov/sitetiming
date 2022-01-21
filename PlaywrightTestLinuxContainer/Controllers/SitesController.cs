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
        public async Task<IActionResult> Get(int take = 10, string? url = null)
        {
            var query = SiteTimingContext.Sites.AsQueryable();
              

            if (!string.IsNullOrEmpty(url) && !url.StartsWith("http"))
            {
                url = $"http://{url}";
            }

            if (!string.IsNullOrEmpty(url))
            {
                query = query.Where(s => s.Url == url);
            }

            query = query.Include(s => s.Probes)
                .OrderByDescending(s => s.Id)
                .Take(take);

            var result = await query
                .ToListAsync();

            result = result.Select(result =>
            {
                result.LastProbe = result.Probes?.OrderByDescending(s => s.Id)?.FirstOrDefault();
                return result;

            }).ToList();

            var jr = new JsonResult(result);
            return jr;
        }

    }
}