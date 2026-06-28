using MediatR;

namespace MarketHub.Application.Features.Inventories.Queries.GetInventoryByProductId;

public class GetInventoryByProductIdQuery : IRequest<GetInventoryByProductIdQueryResponse>
{
    public Guid ProductId { get; set; }
    public bool TrackChanges { get; set; }
}