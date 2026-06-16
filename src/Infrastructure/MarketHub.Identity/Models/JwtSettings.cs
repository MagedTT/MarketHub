namespace MarketHub.Identity.Models;

public class JwtSettings
{
    public readonly string Section = "JwtSettings";

    public string ValidIssuer { get; set; } = string.Empty;
    public string ValidAudience { get; set; } = string.Empty;
    public string ExpiresAt { get; set; } = string.Empty;
}