using MediatR;

namespace MarketHub.Application.Features.Products.Queries.GetTotalProductsOutOfStockForStore;

public class GetTotalProductsOutOfStockForStoreQuery : IRequest<GetTotalProductsOutOfStockForStoreQueryResponse>
{
    public Guid StoreId { get; set; }
}