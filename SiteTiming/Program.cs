using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.ResponseCompression;
using PlaywrightTestLinuxContainer;
using SiteTiming.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddDbContext<SiteTimingContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Clear();
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();

    var mimeTypes = ResponseCompressionDefaults.MimeTypes;
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "document", "text/html", "image/x-icon" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();

//this is required for dots
app.MapFallbackToPage("/sites/{*UrlToGetData}", "/_Host");
//app.MapFallbackToPage("/_Host");
//app.MapFallbackToPage("/{*UrlToGetData}", "/_Host");

//app.Use(async (context, next) =>
//{
//    // Problems in Blazor when path container https:// or http://
//    var path = context.Request.Path.ToString();


//if (path != "blazor.server.js" && path.EndsWith("blazor.server.js"))
//{
//    var newUrl = $"{context.Request.Scheme}://{context.Request.Host}/_framework/blazor.server.js";
//    context.Response.Redirect(newUrl, true);
//}
////_blazor/initializers
//if (path != "/_blazor/initializers" & path.EndsWith("/_blazor/initializers"))
//{
//    var newUrl = $"{context.Request.Scheme}://{context.Request.Host}/_blazor/initializers";
//    context.Response.Redirect(newUrl, true);
//}

////Request URL: https://localhost:7267/github.com/dotnet/aspnetcore/issues/_blazor/negotiate?negotiateVersion=1

//if (path.Contains("negotiate"))
//{
//}

//if (path != "/_blazor/negotiate" & path.EndsWith("/_blazor/negotiate"))
//{
//    var newUrl = $"{context.Request.Scheme}://{context.Request.Host}/_blazor/negotiate{context.Request.QueryString}";
//    context.Response.Redirect(newUrl, true);
//}

//if (path.Contains("http://") || path.Contains("https://"))
//{
//    path = path.Replace("http://", string.Empty).Replace("https://", string.Empty);

//    var newUrl = $"{context.Request.Scheme}://{context.Request.Host}{path}";
//    context.Response.Redirect(newUrl, true);


//}



//    await next.Invoke();
//});


app.Run();
