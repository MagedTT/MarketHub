using MarketHub.Application.Models.Mail;
using SendGrid;

namespace MarketHub.Application.Contracts.Infrastructure;

public interface IEmailService
{
    Task<Response> SendEmailAsync(Email email);
}