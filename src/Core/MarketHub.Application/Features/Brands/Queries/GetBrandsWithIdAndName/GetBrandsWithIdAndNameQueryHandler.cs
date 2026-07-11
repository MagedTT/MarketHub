using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Brands.Queries.GetBrandsWithIdAndName;

public class GetBrandsWithIdAndNameQueryHandler : IRequestHandler<GetBrandsWithIdAndNameQuery, IEnumerable<BrandDto>>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetBrandsWithIdAndNameQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<IEnumerable<BrandDto>> Handle(GetBrandsWithIdAndNameQuery request, CancellationToken cancellationToken)
        => await _repositoryManager.BrandRepository.GetBrandsWithIdAndName(request.TrackChanges);

}