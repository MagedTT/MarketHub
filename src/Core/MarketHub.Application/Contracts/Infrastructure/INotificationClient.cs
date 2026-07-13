using MarketHub.Application.Models.Notification;

namespace MarketHub.Application.Contracts.Infrastructure;

public interface INotificationClient
{
    Task ReceiveNotification(NotificationDto notification);
}