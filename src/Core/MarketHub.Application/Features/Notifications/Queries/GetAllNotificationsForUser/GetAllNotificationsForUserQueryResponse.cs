using MarketHub.Application.Models.Notification;
using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Notifications.Queries.GetAllNotificationsForUser;

public class GetAllNotificationsForUserQueryResponse : BaseResponse
{
    public IEnumerable<NotificationDto> Notifications { get; set; } = new List<NotificationDto>();
    public GetAllNotificationsForUserQueryResponse()
        : base()
    { }
}