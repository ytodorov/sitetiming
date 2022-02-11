using Core.Entities;
using Core.GraphQL;
using Core.GraphQL.Types;
using GraphQL.DataLoader;
using GraphQL.MicrosoftDI;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Playwright;
using Mitsubishi.MCMachinery.Core.GraphQL;
using Mitsubishi.MCMachinery.Core.GraphQL.Types;
using Newtonsoft.Json;
using PlaywrightTestLinuxContainer;
using System.Net;

using (SiteTimingContext context = new SiteTimingContext())
{
    context.Database.Migrate();
}

//var siteMap = HelperMethods.GetSiteMap();

HelperMethods.PopulateSitesInDatabaseFromFile();
HelperMethods.IpAddressOfServer = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(s => s.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
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


//builder.Services.AddLogging(builder => builder.AddConsole());
builder.Services.AddHttpContextAccessor();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Clear();
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();

    var mimeTypes = ResponseCompressionDefaults.MimeTypes;
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "document", "text/html", "image/x-icon" });
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

ConfigureGraphQL(builder.Services);

var app = builder.Build();



// use graphql-playground at default url /ui/playground
app.UseGraphQLPlayground(new PlaygroundOptions() {  SchemaPollingEnabled = false });

app.UseHttpsRedirection();

app.UseResponseCompression();

app.UseSwagger();
app.UseSwaggerUI();

// add http for Schema at default url /graphql
app.UseGraphQL<ISchema>();

//app.UseAuthorization();

app.UseCors("AllowClient");

app.MapControllers();

app.MapGet("/", (Func<string>)(() => $"It is working on {Environment.MachineName} {Environment.OSVersion}"));

app.Run();

void ConfigureGraphQL(IServiceCollection services)
{
    services.AddSingleton<MyEntityDataLoader<SiteEntity>>();

    services.AddSingleton<SiteTimingQuery>();

    services.AddSingleton<SiteObjectGraphType>();
    services.AddSingleton<ProbeObjectGraphType>();

    services.AddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>();
    services.AddSingleton<DataLoaderDocumentListener>();


    services.AddSingleton<ISchema, SiteTimingSchema>();

    //services.AddGraphQL();

    services.AddGraphQL(options =>
    {
        //options.EnableMetrics = true;
    })
    .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
    .AddSystemTextJson()
    .AddUserContextBuilder(httpContext => new GraphQLUserContext { User = httpContext.User });
}