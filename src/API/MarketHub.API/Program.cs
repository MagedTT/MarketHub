using MarketHub.API;
using MarketHub.Infrastructure.Notification.Hub;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplication app = builder
    .ConfigureServices()
    .ConfigurePipeline();

app.UseSerilogRequestLogging();

app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();