using MarketHub.Application.DTOs.Persistence.Orders;
using MarketHub.Application.Responses;
using MarketHub.Application.Shared;

namespace MarketHub.Application.Features.Orders.Queries.GetRecentOrdersForStore;

public class GetRecentOrdersForStoreCommandResponse : BaseResponse
{
    public MetaData MetaData { get; set; } = new();
    public IEnumerable<StoreOrderDto> Orders { get; set; } = new List<StoreOrderDto>();

    public GetRecentOrdersForStoreCommandResponse()
        : base()
    { }
}