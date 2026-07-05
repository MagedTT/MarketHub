using MediatR;

namespace MarketHub.Application.Features.Carts.Queries.GetAmountInCart;

public class GetAmountInCartQuery : IRequest<GetAmountInCartQueryResponse>
{
    public Guid UserId { get; set; }
}