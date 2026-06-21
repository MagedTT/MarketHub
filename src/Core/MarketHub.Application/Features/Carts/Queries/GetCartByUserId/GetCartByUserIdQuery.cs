using MediatR;

namespace MarketHub.Application.Features.Carts.Queries.GetCartByUserId;

public class GetCartByUserIdQuery : IRequest<GetCartByUserIdQueryResponse>
{
    public Guid UserId { get; set; }
}