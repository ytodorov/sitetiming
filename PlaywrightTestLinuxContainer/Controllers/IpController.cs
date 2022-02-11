using Core.Entities;
using Core.Redis;
using IpInfo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Playwright;
using StackExchange.Redis;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlaywrightTestLinuxContainer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IpController : MyControllerBase
    {
        public IpController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        [HttpGet(Name = "GetIp")]
        public async Task<IActionResult> GetIp()
        {
            try
            {
                string stringIp = "77.70.29.69"; // HttpContext.Connection.RemoteIpAddress.ToString();

                RedisValue ipDetailsAsJson = RedisConnection.Connection.GetDatabase().StringGet(stringIp);

                if (ipDetailsAsJson.IsNullOrEmpty)
                {
                    HttpClient httpClient = ServiceProvider.GetRequiredService<HttpClient>();
                    IpInfoApi api = new IpInfoApi("6a852f28bb9103", httpClient);

                    FullResponse response = await api.GetInformationByIpAsync(stringIp);

                    string responseAsJson = JsonSerializer.Serialize(response);

                    RedisConnection.Connection.GetDatabase().StringSet(stringIp, responseAsJson);

                    JsonResult js = new JsonResult(response);
                    return js;
                }
                else
                {
                    FullResponse response = JsonSerializer.Deserialize<FullResponse>(ipDetailsAsJson);
                    JsonResult js = new JsonResult(response);
                    return js;
                }
            }
            catch (Exception ex)
            {
                JsonResult js = new JsonResult(new FullResponse() { Ip = ex.Message });
                return js;
            }
        }


    }
}