using MarketHub.Application.DTOs.Persistence.Orders;
using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Orders.Queries.GetOrderStatusCountForStore;

public class GetOrderStatusCountForStoreQueryResponse : BaseResponse
{
    public IEnumerable<StoreOrderStatusCount> StoreOrderStatusCounts { get; set; } = new List<StoreOrderStatusCount>();

    public GetOrderStatusCountForStoreQueryResponse()
        : base()
    { }
}