using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Stores.Queries.GetStoreIdAndStatus;

public class GetStoreIdAnsActiveStatusQueryHandler : IRequestHandler<GetStoreIdAnsActiveStatusQuery, StoreStatusDto?>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetStoreIdAnsActiveStatusQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<StoreStatusDto?> Handle(GetStoreIdAnsActiveStatusQuery request, CancellationToken cancellationToken)
        => await _repositoryManager.StoreRepository.GetStoreIdAndActiveStatusAsync(request.UserId);
}