using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Inventories.Commans.UpdateInventory;

public class UpdateInventoryCommand : IRequest<BaseResponse>
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int AvailableQuantity { get; set; }
    public int ReservedQuantity { get; set; }
}