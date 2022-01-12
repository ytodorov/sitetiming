using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Playwright;
using Newtonsoft.Json;
using PlaywrightTestLinuxContainer.Entities;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlaywrightTestLinuxContainer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private SiteTimingContext timingContext;

        private readonly ILogger<TimingController> _logger;

        public DataController(ILogger<TimingController> logger, SiteTimingContext timingContext)
        {
            _logger = logger;
            this.timingContext = timingContext;
        }

        [HttpGet(Name = "GetData")]
        public async Task<string> Get()
        {
            var location = Assembly.GetExecutingAssembly().Location;

            FileInfo fileInfo = new FileInfo(location);
            var files = Directory.GetFiles(fileInfo.Directory.FullName, "*", SearchOption.AllDirectories);
            var fileInfos = files.Select(f => new FileInfo(f)).OrderByDescending(f => f.LastWriteTime).ToList();
            
            StringBuilder sb = new StringBuilder();

            foreach (var fi in fileInfos)
            {
                sb.AppendLine($"{fi.Name} {fi.LastWriteTimeUtc.ToString(CultureInfo.GetCultureInfo("bg-BG"))} UTC");
            }

            return sb.ToString();
        }

       
    }
}