using MediatR;

namespace MarketHub.Application.Features.Brands.Queries.GetBrandsWithIdAndName;

public class GetBrandsWithIdAndNameQuery : IRequest<IEnumerable<BrandDto>>
{
    public bool TrackChanges { get; set; }
}