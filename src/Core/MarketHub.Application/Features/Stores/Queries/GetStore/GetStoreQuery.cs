using MediatR;

namespace MarketHub.Application.Features.Stores.Queries.GetStore;

public class GetStoreQuery : IRequest<GetStoreQueryResponse>
{
    public Guid StoreId { get; set; }
}