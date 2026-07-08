using MediatR;

namespace MarketHub.Application.Features.Products.Queries.GetTotalProductsInStockForStore;

public class GetTotalProductsInStockForStoreQuery : IRequest<GetTotalProductsInStockForStoreQueryResponse>
{
    public Guid StoreId { get; set; }
}