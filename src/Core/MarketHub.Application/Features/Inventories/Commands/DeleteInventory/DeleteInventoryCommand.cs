using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Inventories.Commands.DeleteInventory;

public class DeleteInventoryCommand : IRequest<BaseResponse>
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
}