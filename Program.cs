using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using mygallery.Context;
using mygallery.Data;
using mygallery.Infrastuctures;
using mygallery.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.File("./logs/bootstraplog-.txt",
        LogEventLevel.Verbose,
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 14,
        fileSizeLimitBytes: 100_000_000)
    .CreateBootstrapLogger();

Log.Logger = new LoggerConfiguration()
.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
.Enrich.FromLogContext()
.WriteTo.Console()
.WriteTo.Debug()
.WriteTo.File(new JsonFormatter(), "./logs/log-.json", LogEventLevel.Warning,
encoding: Encoding.UTF8,
rollingInterval: RollingInterval.Day,
fileSizeLimitBytes: 10000000,
retainedFileCountLimit: 30)
.CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
#region Services
// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/giris";
        options.Cookie.Name = "MYGALLERY_AUTH";
        options.AccessDeniedPath = "/home/denied";
        options.Cookie.Path = "/";
        options.Cookie.HttpOnly = true;
    });


builder.Services.AddSession();
builder.Services.AddMemoryCache();
var configuration = builder.Configuration;

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton(configuration);
builder.Services.Configure<AppConfig>(configuration.GetSection("AppConfig"));


var cs = configuration.GetSection("AppConfig").GetValue<string>("ConnectionString");

int sqlCompatibilityLevel;

// Create a SqlConnection to get the compatibility level
using (var connection = new SqlConnection(cs))
{
    connection.Open();

    using (var command = new SqlCommand("SELECT compatibility_level FROM sys.databases WHERE name = DB_NAME()", connection))
    {
        var result = command.ExecuteScalar();

        if (result != null)
        {
            try
            {
                sqlCompatibilityLevel = Convert.ToInt32(result);
            }
            catch (InvalidCastException ex)
            {
                Log.Error("Error while converting sqlCompatibilityLevel to int: " + ex.Message);
                sqlCompatibilityLevel = 110;
            }
        }
        else
        {
            Log.Error("sqlCompatibilityLevel query result is null.");
            sqlCompatibilityLevel = 110;
        }
    }
}

builder.Services.AddDbContext<MyGalleryContext>(options =>
options.UseLazyLoadingProxies()
       .UseSqlServer(cs, o => o.UseCompatibilityLevel(sqlCompatibilityLevel))
       .ConfigureWarnings(b => b.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS)));
builder.Services.AddApplicationInsightsTelemetry();

#endregion
var app = builder.Build();
#region Middleware
var env = app.Environment;
var appConfig = app.Services.GetRequiredService<IOptions<AppConfig>>();

AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(env.ContentRootPath, "App_Data"));

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");
    app.UseHsts();
}
app.UseSession();

app.UseStaticFiles();

// app.UseSerilogRequestLogging(opts
//             => opts.EnrichDiagnosticContext = UnhandledExceptionMiddleware.EnrichFromRequest);

app.UseMiddleware<UnhandledExceptionMiddleware>();

#endregion
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=home}/{action=index}");

app.MapControllerRoute(
    "detail",
    "{controller}/{id}/{action}");
app.Run();
