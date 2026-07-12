using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Carts.Queries.GetTotalPriceInCart;

public class GetTotalPriceInCartQueryHandler : IRequestHandler<GetTotalPriceInCartQuery, decimal>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetTotalPriceInCartQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<decimal> Handle(GetTotalPriceInCartQuery request, CancellationToken cancellationToken)
        => await _repositoryManager.CartRepository.GetTotalPriceInCartByUserIdAsync(request.CartId);
}