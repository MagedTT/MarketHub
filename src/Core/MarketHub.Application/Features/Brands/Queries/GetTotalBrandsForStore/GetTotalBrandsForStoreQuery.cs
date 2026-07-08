using MediatR;

namespace MarketHub.Application.Features.Brands.Queries.GetTotalBrandsForStore;

public class GetTotalBrandsForStoreQuery : IRequest<int>
{
    public Guid StoreId { get; set; }
}