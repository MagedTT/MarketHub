using MarketHub.Application;
using MarketHub.Identity;
using MarketHub.Infrastructure;
using MarketHub.Persistence;
using Serilog;

namespace MarketHub.API;

public static class Extensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Host.UseSerilog((HostBuilderContext hostBuilderContext, IServiceProvider serviceProvider, LoggerConfiguration loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(hostBuilderContext.Configuration)
                .ReadFrom.Services(serviceProvider);
        });

        builder.Services.AddPersistenceServices(builder.Configuration);
        builder.Services.AddAutoMapper(configs =>
        {
            configs.AddProfile<ApiMappingProfile>();
        });

        builder.Services.AddIdentityServices(builder.Configuration);
        builder.Services.AddApplicationLayerServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}