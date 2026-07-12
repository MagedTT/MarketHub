using MediatR;

namespace MarketHub.Application.Features.Carts.Queries.GetTotalPriceInCart;

public class GetTotalPriceInCartQuery : IRequest<decimal>
{
    public Guid CartId { get; set; }
}