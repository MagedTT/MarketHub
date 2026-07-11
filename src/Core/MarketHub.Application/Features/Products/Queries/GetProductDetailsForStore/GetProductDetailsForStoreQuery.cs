using MediatR;

namespace MarketHub.Application.Features.Products.Queries.GetTotalProductsForStore;

public class GetProductDetailsForStoreQuery : IRequest<GetProductDetailsForStoreQueryResponse>
{
    public Guid StoreId { get; set; }
    public Guid ProductId { get; set; }
}