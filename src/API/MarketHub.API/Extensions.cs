using System.Text;
using MarketHub.Application;
using MarketHub.Identity;
using MarketHub.Infrastructure;
using MarketHub.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
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

        builder.Services.AddAuthentication(config =>
        {
            config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = builder.Configuration["JwtSettings:ValidIssuer"],
                ValidAudience = builder.Configuration["JwtSettings:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("MarketHub__SigningKey")!))
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    StringValues token = context.Request.Query["access_token"];
                    PathString path = context.HttpContext.Request.Path;

                    if (!string.IsNullOrWhiteSpace(token) && path.StartsWithSegments("/hubs"))
                        context.Token = token;

                    return Task.CompletedTask;
                }
            };
        });

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

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}