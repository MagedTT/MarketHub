using MediatR;

namespace MarketHub.Application.Features.Products.GetTopNBestSellingProductsForStore;

public class GetTopNBestSellingProductsForStoreQuery : IRequest<GetTopNBestSellingProductsForStoreQueryResponse>
{
    public Guid StoreId { get; set; }
    public int N { get; set; }
}