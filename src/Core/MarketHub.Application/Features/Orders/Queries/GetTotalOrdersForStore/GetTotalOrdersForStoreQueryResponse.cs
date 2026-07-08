using MarketHub.Application.DTOs.Persistence.Orders;
using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Orders.Queries.GetTotalOrdersForStore;

public class GetTotalOrdersForStoreQueryResponse : BaseResponse
{
    public int TotalOrders { get; set; } = new();

    public GetTotalOrdersForStoreQueryResponse()
        : base()
    { }
}