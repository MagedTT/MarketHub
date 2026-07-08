using MarketHub.Application.DTOs.Persistence.Product;
using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Brands.Queries.GetTotalBrandsForStore;

public class GetTopNSellingBrandsForStoreQueryResponse : BaseResponse
{
    public IEnumerable<TopBrandDto> Brands { get; set; } = new List<TopBrandDto>();
    public GetTopNSellingBrandsForStoreQueryResponse()
        : base()
    { }
}