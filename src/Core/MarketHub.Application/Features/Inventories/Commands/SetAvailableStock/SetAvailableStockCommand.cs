using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Inventories.Commands.SetAvailableStock;

public class SetAvailableStockCommand : IRequest<BaseResponse>
{
    public Guid InventoryId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}