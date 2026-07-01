using MarketHub.Application.DTOs.Persistence.Orders;
using MarketHub.Application.Responses;
using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Orders.Queries.GetOrders;

public class GetOrdersQueryResponse : BaseResponse
{
    public MetaData MetaData { get; set; } = new();
    public IEnumerable<OrderDto> Orders { get; set; } = new List<OrderDto>();

    public GetOrdersQueryResponse()
        : base()
    { }
}