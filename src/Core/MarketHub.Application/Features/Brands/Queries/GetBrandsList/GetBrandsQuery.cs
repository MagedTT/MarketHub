using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Brands.Queries.GetBrandsList;

public class GetBrandsQuery : IRequest<(IEnumerable<Brand>, MetaData metaData)>
{
    public bool TrackChanges { get; set; }
    public BrandParameters BrandParameters { get; set; } = new();
}