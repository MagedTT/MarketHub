using MarketHub.Application.DTOs.Persistence.Product;
using MediatR;

namespace MarketHub.Application.Features.Brands.Queries.GetTotalBrandsForStore;

public class GetTopNSellingBrandsForStoreQuery : IRequest<GetTopNSellingBrandsForStoreQueryResponse>
{
    public Guid StoreId { get; set; }
    public int N { get; set; }
}