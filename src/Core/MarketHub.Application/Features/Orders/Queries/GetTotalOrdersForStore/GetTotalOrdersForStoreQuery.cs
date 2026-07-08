using MediatR;

namespace MarketHub.Application.Features.Orders.Queries.GetTotalOrdersForStore;

public class GetTotalOrdersForStoreQuery : IRequest<GetTotalOrdersForStoreQueryResponse>
{
    public Guid StoreId { get; set; }
}