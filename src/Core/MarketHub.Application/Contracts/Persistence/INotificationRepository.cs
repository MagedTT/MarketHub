using MarketHub.Application.Models.Notification;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface INotificationRepository
{
    Task<Notification?> GetNotificationByIdAsync(Guid notificationId, bool trackChanges);
    Task<IEnumerable<NotificationDto>> GetAllNotificationsByUserIdAsync(Guid userId, bool trackChanges);
    Task MarkAllNotificationsAsReadByUserIdAsync(Guid userId);
    Task<bool> NotificationExistsByIdAndUserIdAsync(Guid notificationId, Guid userId);
    Task MarkNotificationAsReadAsync(Guid notificationId);
    void CreateNotification(Notification notification);
}