
using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Products.Queries.GetTotalProductsInStockForStore;

public class GetTotalProductsInStockForStoreQueryResponse : BaseResponse
{
    public int TotalProductsInStock { get; set; }
    public GetTotalProductsInStockForStoreQueryResponse()
        : base()
    { }
}