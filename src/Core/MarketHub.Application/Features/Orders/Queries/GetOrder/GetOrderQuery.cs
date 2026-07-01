using MediatR;

namespace MarketHub.Application.Features.Orders.Queries.GetOrder;

public class GetOrderQuery : IRequest<GetOrderQueryResponse>
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
    public bool TrackChanges { get; set; } = false;
}