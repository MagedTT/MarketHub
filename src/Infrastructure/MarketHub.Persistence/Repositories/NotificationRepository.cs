using System.Security.Cryptography.X509Certificates;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Models.Notification;
using MarketHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Persistence.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly MarketHubDbContext _context;
    public NotificationRepository(MarketHubDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<NotificationDto>> GetAllNotificationsByUserIdAsync(Guid userId, bool trackChanges)
    {
        IQueryable<Notification> notifications = _context.Notifications;

        if (!trackChanges)
            notifications = notifications.AsNoTracking();

        notifications = notifications.Where(x => x.UserId == userId);

        List<NotificationDto> notificationDtos = await notifications
            .Select(x => new NotificationDto
            {
                Id = x.Id,
                UserId = x.UserId,
                ReferenceId = x.ReferenceId,
                Title = x.Title,
                Message = x.Message,
                Type = x.Type,
                IsRead = x.IsRead,
                CreatedAt = x.CreatedAt
            }).ToListAsync();

        return notificationDtos;
    }

    public async Task<Notification?> GetNotificationByIdAsync(Guid notificationId, bool trackChanges)
    {
        IQueryable<Notification> notifications = _context.Notifications;

        if (!trackChanges)
            notifications = notifications.AsNoTracking();

        return await notifications.FirstOrDefaultAsync(x => x.Id == notificationId);
    }

    public async Task MarkAllNotificationsAsReadByUserIdAsync(Guid userId)
        => await _context.Notifications.Where(x => x.UserId == userId && !x.IsRead)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.IsRead, true));

    public void CreateNotification(Notification notification)
        => _context.Notifications.Add(notification);

    public async Task MarkNotificationAsReadAsync(Guid notificationId)
        => await _context.Notifications.Where(x => x.Id == notificationId && !x.IsRead)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.IsRead, true));

    public async Task<bool> NotificationExistsByIdAndUserIdAsync(Guid notificationId, Guid userId)
        => await _context.Notifications.AnyAsync(x => x.Id == notificationId && x.UserId == userId);
}