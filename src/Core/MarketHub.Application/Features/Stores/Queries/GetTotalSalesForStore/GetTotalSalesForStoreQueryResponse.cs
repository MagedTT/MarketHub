using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Stores.Queries.GetTotalSalesForStore;

public class GetTotalSalesForStoreQueryResponse : BaseResponse
{
    public decimal TotalSales { get; set; }
    public GetTotalSalesForStoreQueryResponse()
        : base()
    { }
}