using MarketHub.Application.DTOs.Persistence.Orders;
using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Orders.Queries.GetOrderForStore;

public class GetOrderForStoreQueryResponse : BaseResponse
{
    public StoreOrderDto? Order { get; set; }
    public GetOrderForStoreQueryResponse()
        : base()
    { }
}