using MediatR;

namespace MarketHub.Application.Features.Brands.Queries.GetBrandById;

public class GetBrandByIdQuery : IRequest<GetBrandByIdQueryResponse>
{
    public Guid BrandId { get; set; }
    public bool TrackChanges { get; set; }
}