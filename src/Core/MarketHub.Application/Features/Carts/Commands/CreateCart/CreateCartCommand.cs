using MediatR;

namespace MarketHub.Application.Features.Carts.Commands.CreateCart;

public class CreateCartCommand : IRequest<CreateCartCommandResponse>
{
    public Guid UserId { get; set; }
}