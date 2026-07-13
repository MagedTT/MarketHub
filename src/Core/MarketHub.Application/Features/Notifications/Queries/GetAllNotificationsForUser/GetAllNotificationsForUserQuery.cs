using MediatR;

namespace MarketHub.Application.Features.Notifications.Queries.GetAllNotificationsForUser;

public class GetAllNotificationsForUserQuery : IRequest<GetAllNotificationsForUserQueryResponse>
{
    public Guid UserId { get; set; }
    public bool TrackChanges { get; set; }
}