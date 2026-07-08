
using MarketHub.Application.DTOs.Persistence.Product;
using MarketHub.Application.Responses;
using MarketHub.Application.Shared;

namespace MarketHub.Application.Features.Products.Queries.GetAllProductsForStore;

public class GetAllProductsForStoreQueryResponse : BaseResponse
{
    public MetaData MetaData { get; set; } = new();
    public IEnumerable<ProductDto> Products { get; set; } = new List<ProductDto>();
    public GetAllProductsForStoreQueryResponse()
        : base()
    { }
}