namespace MarketHub.Application.Models.Mail;

public class Email
{
    public string ToName { get; set; } = string.Empty;
    public string ToEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string HtmlContent { get; set; } = string.Empty;
}