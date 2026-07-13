using MarketHub.Application.Models.Notification;

namespace MarketHub.Application.Contracts.Infrastructure;

public interface INotificationService
{
    Task SendToUserAsync(string userId, NotificationDto notification);
}