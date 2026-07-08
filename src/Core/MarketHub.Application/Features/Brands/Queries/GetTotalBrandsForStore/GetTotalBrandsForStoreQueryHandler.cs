using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Brands.Queries.GetTotalBrandsForStore;

public class GetTotalBrandsForStoreQueryHandler : IRequestHandler<GetTotalBrandsForStoreQuery, int>
{
    private IRepositoryManager _repositoryManager;
    public GetTotalBrandsForStoreQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<int> Handle(GetTotalBrandsForStoreQuery request, CancellationToken cancellationToken)
        => await _repositoryManager.BrandRepository.TotalBrandsByStoreIdAsync(request.StoreId);
}