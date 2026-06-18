using MediatR;

namespace MarketHub.Application.Features.Products.Queries.GetProductCard;

public class GetProductCardQuery : IRequest<GetProductCardQueryResponse>
{
    public Guid ProductId { get; set; }
    public bool TrackChanges { get; set; } = false;
}