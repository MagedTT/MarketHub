using MarketHub.API;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplication app = builder
    .ConfigureServices()
    .ConfigurePipeline();

app.UseSerilogRequestLogging();

app.Run();