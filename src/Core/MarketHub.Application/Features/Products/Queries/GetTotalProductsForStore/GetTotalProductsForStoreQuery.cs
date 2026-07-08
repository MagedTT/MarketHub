using MediatR;

namespace MarketHub.Application.Features.Products.Queries.GetTotalProductsForStore;

public class GetTotalProductsForStoreQuery : IRequest<GetTotalProductsForStoreQueryResponse>
{
    public Guid StoreId { get; set; }
}