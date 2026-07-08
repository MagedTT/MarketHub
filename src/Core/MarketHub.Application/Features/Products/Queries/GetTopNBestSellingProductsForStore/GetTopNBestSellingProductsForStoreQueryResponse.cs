using MarketHub.Application.DTOs.Persistence.Product;
using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Products.GetTopNBestSellingProductsForStore;

public class GetTopNBestSellingProductsForStoreQueryResponse : BaseResponse
{
    public IEnumerable<ProductDto> Products { get; set; } = new List<ProductDto>();
    public GetTopNBestSellingProductsForStoreQueryResponse()
        : base()
    { }
}