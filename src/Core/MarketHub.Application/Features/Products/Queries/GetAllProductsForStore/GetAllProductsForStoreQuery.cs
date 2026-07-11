using MarketHub.Application.Shared;
using MediatR;

namespace MarketHub.Application.Features.Products.Queries.GetAllProductsForStore;

public class GetAllProductsForStoreQuery : IRequest<GetAllProductsForStoreQueryResponse>
{
    public Guid StoreId { get; set; }
    public int? ProductStatus { get; set; } = 0;
    public StoreProductsParameters StoreProductParameters { get; set; } = new();
}