using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Inventories.COmmands.CreateInventory;

public class CreateInventoryCommand : IRequest<BaseResponse>
{
    public Guid ProductId { get; set; }
    public int AvailableQuantity { get; set; }
}