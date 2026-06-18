using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Profiles;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MarketHub.Application;

public static class ApplicationServiceRegisteration
{
    public static IServiceCollection AddApplicationLayerServices(this IServiceCollection services)
    {
        services.AddAutoMapper(configs =>
        {
            configs.AddProfile<MappingProfile>();
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationServiceRegisteration).Assembly);
        });

        return services;
    }
}