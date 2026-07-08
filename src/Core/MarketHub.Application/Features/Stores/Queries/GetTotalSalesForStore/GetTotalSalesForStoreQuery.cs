using MediatR;

namespace MarketHub.Application.Features.Stores.Queries.GetTotalSalesForStore;

public class GetTotalSalesForStoreQuery : IRequest<GetTotalSalesForStoreQueryResponse>
{
    public Guid StoreId { get; set; }
}