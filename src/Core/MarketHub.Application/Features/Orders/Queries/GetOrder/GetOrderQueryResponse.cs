using MarketHub.Application.DTOs.Persistence.Orders;
using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Orders.Queries.GetOrder;

public class GetOrderQueryResponse : BaseResponse
{
    public OrderDto? Order { get; set; }
    public GetOrderQueryResponse()
        : base()
    { }
}