
using MarketHub.Application.DTOs.Persistence.Product;
using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Products.Queries.GetTotalProductsForStore;

public class GetProductDetailsForStoreQueryResponse : BaseResponse
{
    public StoreProductDetailsDto? ProductDetails { get; set; }
    public GetProductDetailsForStoreQueryResponse()
        : base()
    { }
}