using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Brands.Queries.GetBrandsList;

public class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, (IEnumerable<Brand> brands, MetaData metaData)>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetBrandsQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<(IEnumerable<Brand> brands, MetaData metaData)> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
    {
        PagedList<Brand> response = await _repositoryManager.BrandRepository.GetBrandsAsync(request.BrandParameters, request.TrackChanges);

        return (brands: response.AsEnumerable(), metaData: response.MetaData);
    }
}