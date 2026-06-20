using MarketHub.Application.Shared;
using MediatR;

namespace MarketHub.Application.Features.Brands.Queries.GetBrandsNames;

public class GetBrandsNamesQuery : IRequest<(IEnumerable<string> brandsNames, MetaData metaData)>
{
    public bool TrackChanges { get; set; }
    public BrandParameters BrandParameters { get; set; } = new();
}