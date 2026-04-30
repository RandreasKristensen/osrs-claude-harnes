using Microsoft.EntityFrameworkCore;
using Monitor.Web.Api;
using Monitor.Web.Data;
using Monitor.Web.Poller;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MonitorDb>(opt => opt.UseSqlite("Data Source=monitor.db"));
builder.Services.AddHostedService<HealthPoller>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/health", () => "OK");
app.MapDashboard();

app.Run();
