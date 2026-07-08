
using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Products.Queries.GetTotalProductsOutOfStockForStore;

public class GetTotalProductsOutOfStockForStoreQueryResponse : BaseResponse
{
    public int TotalProductsOutOfStock { get; set; }
    public GetTotalProductsOutOfStockForStoreQueryResponse()
        : base()
    { }
}