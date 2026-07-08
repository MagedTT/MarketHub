using MarketHub.Domain.Enums;

namespace MarketHub.Application.Shared;

public class StoreOrdersParameters : RequestParameters
{
    public OrderStatus? OrderStatus { get; set; } = null;
}