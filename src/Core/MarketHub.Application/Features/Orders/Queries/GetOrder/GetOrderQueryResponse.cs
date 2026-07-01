using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Features.Orders.Queries.GetOrder;

public class GetOrderQueryResponse : BaseResponse
{
    public Order? Order { get; set; }
    public GetOrderQueryResponse()
        : base()
    { }
}