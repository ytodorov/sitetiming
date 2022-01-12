using Microsoft.EntityFrameworkCore;
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
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SiteTimingContext>();
builder.Services.AddHostedService<TimedHostedService>();

// The following line enables Application Insights telemetry collection.
builder.Services.AddApplicationInsightsTelemetry("e376512e-2b93-4864-bcf3-b4c103a3c374");

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", (Func<string>)(() => $"It is working on {Environment.MachineName} {Environment.OSVersion}"));

app.Run();
