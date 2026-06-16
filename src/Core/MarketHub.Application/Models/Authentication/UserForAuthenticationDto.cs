namespace MarketHub.Application.Models.Authentication;

public class UserForAuthenticationDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}