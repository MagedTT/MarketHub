using MarketHub.Application.Contracts.Identity;
using MarketHub.Identity.Models;
using MarketHub.Identity.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarketHub.Identity;

public static class IdentityServiceRegisteration
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.AddTransient<IAuthenticationService, AuthenticationService>();

        return services;
    }
}