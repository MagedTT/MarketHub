using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Notifications.Commands.MarkAllNotificationAsReadForUser;

public class MarkAllNotificationAsReadForUserCommand : IRequest<BaseResponse>
{
    public Guid UserId { get; set; }
}