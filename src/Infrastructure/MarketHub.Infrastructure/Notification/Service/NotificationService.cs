using MarketHub.Application.Contracts.Infrastructure;
using MarketHub.Application.Models.Notification;
using MarketHub.Infrastructure.Notification.Hub;
using Microsoft.AspNetCore.SignalR;

namespace MarketHub.Infrastructure.Notification.Service;

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub, INotificationClient> _hub;
    public NotificationService(IHubContext<NotificationHub, INotificationClient> hub)
        => _hub = hub;

    public async Task SendToUserAsync(string userId, NotificationDto notification)
        => await _hub.Clients.User(userId).ReceiveNotification(notification);
}