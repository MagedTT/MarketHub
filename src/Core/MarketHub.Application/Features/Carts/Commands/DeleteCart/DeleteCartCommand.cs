using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Carts.Commands.DeleteCart;

public class DeleteCartCommand : IRequest<BaseResponse>
{
    public Guid UserId { get; set; }
    public Guid CartId { get; set; }
}