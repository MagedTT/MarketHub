using MediatR;

namespace MarketHub.Application.Features.Products.Queries.GetProductDetails;

public class GetProductDetailsQuery : IRequest<GetProductDetailsQueryResponse>
{
    public Guid ProductId { get; set; }
    public bool TrackChanges { get; set; } = false;
}