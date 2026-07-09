using MediatR;

namespace MarketHub.Application.Features.Orders.Queries.GetOrderForStore;

public class GetOrderForStoreQuery : IRequest<GetOrderForStoreQueryResponse>
{
    public Guid OrderId { get; set; }
    public Guid StoreId { get; set; }
}