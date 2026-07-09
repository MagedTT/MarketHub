using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Stores.Queries.GetStoreId;

public class GetStoreIdQueryHandler : IRequestHandler<GetStoreIdQuery, Guid?>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetStoreIdQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<Guid?> Handle(GetStoreIdQuery request, CancellationToken cancellationToken)
        => await _repositoryManager.StoreRepository.GetStoreIdAsync(request.UserId);
}