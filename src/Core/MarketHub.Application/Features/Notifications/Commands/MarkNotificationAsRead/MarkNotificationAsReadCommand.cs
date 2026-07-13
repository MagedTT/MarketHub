using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Notifications.Commands.MarkNotificationAsRead;

public class MarkNotificationAsReadCommand : IRequest<BaseResponse>
{
    public Guid NotificationId { get; set; }
    public Guid UserId { get; set; }
}