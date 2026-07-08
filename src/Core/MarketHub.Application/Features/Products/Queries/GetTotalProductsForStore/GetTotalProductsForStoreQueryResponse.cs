
using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Products.Queries.GetTotalProductsForStore;

public class GetTotalProductsForStoreQueryResponse : BaseResponse
{
    public int TotalProducts { get; set; }
    public GetTotalProductsForStoreQueryResponse()
        : base()
    { }
}