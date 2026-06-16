using MarketHub.Application.Contracts.Infrastructure;
using MarketHub.Application.Models.Mail;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MarketHub.Infrastructure.Mail;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    public EmailService(IOptionsMonitor<EmailSettings> emailSettingsOptionsMonitor)
        => _emailSettings = emailSettingsOptionsMonitor.CurrentValue;

    public async Task<Response> SendEmailAsync(Email email)
    {
        SendGridClient client = new SendGridClient(Environment.GetEnvironmentVariable("MarketHub__SendGrid__ApiKey"));

        EmailAddress to = new EmailAddress
        {
            Name = email.ToName,
            Email = email.ToEmail
        };

        EmailAddress from = new EmailAddress
        {
            Name = _emailSettings.FromName,
            Email = _emailSettings.FromEmail
        };

        SendGridMessage sendGridMessage = MailHelper.CreateSingleEmail(from, to, email.Subject, email.Body, email.HtmlContent);

        Response response = await client.SendEmailAsync(sendGridMessage);

        return response;
    }
}