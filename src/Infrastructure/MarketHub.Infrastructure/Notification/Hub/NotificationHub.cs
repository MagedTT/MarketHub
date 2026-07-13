using System.Security.Claims;
using MarketHub.Application.Contracts.Infrastructure;
using MarketHub.Application.Models.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MarketHub.Infrastructure.Notification.Hub;

[Authorize]
public class NotificationHub : Hub<INotificationClient>
{
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"ConnectionId; {Context.ConnectionId}");
        Console.WriteLine($"IsAuthorized; {Context.User?.Identity?.IsAuthenticated}");
        Console.WriteLine($"UserIdentifier: {Context.UserIdentifier}");

        foreach (Claim claim in Context.User?.Claims ?? [])
            Console.WriteLine($"=> {claim.Type}: {claim.Value}");
    }
}
