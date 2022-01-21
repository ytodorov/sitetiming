using Microsoft.EntityFrameworkCore;
using Microsoft.Playwright;
using Newtonsoft.Json;
using PlaywrightTestLinuxContainer;
using System.Net;

using (SiteTimingContext context = new SiteTimingContext())
{
    context.Database.Migrate();
}

HelperMethods.PopulateSitesInDatabaseFromFile();
HelperMethods.IpAddressOfServer = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(s => s.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SiteTimingContext>();
builder.Services.AddHostedService<TimedHostedService>();
builder.Services.AddSingleton<IBrowser>((s) =>
{
    var playwright = Playwright.CreateAsync().Result;
    var browser = playwright.Chromium.LaunchAsync(
        new() { Headless = true, Timeout = (float)TimeSpan.FromMinutes(2).TotalMilliseconds }).Result;

    return browser;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowClient",
        policy => policy
    ?.SetIsOriginAllowedToAllowWildcardSubdomains()
    ?.AllowAnyOrigin()
    ?.AllowAnyHeader()
    ?.AllowAnyMethod()
    ?.SetIsOriginAllowedToAllowWildcardSubdomains()
    ?.SetPreflightMaxAge(TimeSpan.FromMinutes(20)));
});

// The following line enables Application Insights telemetry collection.
builder.Services.AddApplicationInsightsTelemetry("e376512e-2b93-4864-bcf3-b4c103a3c374");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

//app.UseAuthorization();

app.UseCors("AllowClient");

app.MapControllers();

app.MapGet("/", (Func<string>)(() => $"It is working on {Environment.MachineName} {Environment.OSVersion}"));

app.Run();
