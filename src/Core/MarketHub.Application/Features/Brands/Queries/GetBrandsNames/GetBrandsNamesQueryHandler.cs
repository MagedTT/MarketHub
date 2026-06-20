using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Shared;
using MediatR;

namespace MarketHub.Application.Features.Brands.Queries.GetBrandsNames;

public class GetBrandsNamesQueryHandler : IRequestHandler<GetBrandsNamesQuery, (IEnumerable<string> brandsNames, MetaData metaData)>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetBrandsNamesQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<(IEnumerable<string> brandsNames, MetaData metaData)> Handle(GetBrandsNamesQuery request, CancellationToken cancellationToken)
    {
        PagedList<string> brandsNames = await _repositoryManager.BrandRepository.GetBrandsNamesAsync(request.BrandParameters, request.TrackChanges);

        return (brandsNames: brandsNames.AsEnumerable(), metaData: brandsNames.MetaData);
    }
}