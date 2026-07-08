using MediatR;

namespace MarketHub.Application.Features.Orders.Queries.GetOrderStatusCountForStore;

public class GetOrderStatusCountForStoreQuery : IRequest<GetOrderStatusCountForStoreQueryResponse>
{
    public Guid StoreId { get; set; }
}