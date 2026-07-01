using MarketHub.Application.Shared;
using MediatR;

namespace MarketHub.Application.Features.Orders.Queries.GetOrders;

public class GetOrdersQuery : IRequest<GetOrdersQueryResponse>
{
    public OrderParameters OrderParameters { get; set; } = new();
    public bool TrackChanges { get; set; } = false;
}