using System.Text;
using Serilog;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;

Log.Logger=new LoggerConfiguration()
.MinimumLevel.Override("Microsoft",LogEventLevel.Warning)
.Enrich.FromLogContext()
.WriteTo.Console()
.WriteTo.Debug()
.WriteTo.File(new JsonFormatter(),"./logs/log-.json",LogEventLevel.Warning,
encoding:Encoding.UTF8,
rollingInterval:RollingInterval.Day,
fileSizeLimitBytes:10000000,
retainedFileCountLimit:30)
.CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

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
