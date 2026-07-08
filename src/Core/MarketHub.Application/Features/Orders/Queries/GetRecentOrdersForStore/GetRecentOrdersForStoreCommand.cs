using MarketHub.Application.Shared;
using MediatR;

namespace MarketHub.Application.Features.Orders.Queries.GetRecentOrdersForStore;

public class GetRecentOrdersForStoreCommand : IRequest<GetRecentOrdersForStoreCommandResponse>
{
    public Guid StoreId { get; set; }
    public StoreOrdersParameters StoreOrdersParameters { get; set; } = new();
}