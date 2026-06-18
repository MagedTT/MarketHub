using MarketHub.Application.Contracts.Infrastructure;
using MarketHub.Application.Models.Mail;
using MarketHub.Infrastructure.FileUpload;
using MarketHub.Infrastructure.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarketHub.Infrastructure;

public static class InfrastructureServiceRegisteration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        services.AddScoped<IFileUploadService, FileUploadService>();
        services.AddTransient<IEmailService, EmailService>();

        return services;
    }
}