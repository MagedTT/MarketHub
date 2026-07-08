
using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Products.Queries.GetTotalProductsInStockForStore;

public class GetTotalProductsInStockForStoreQueryValidator : AbstractValidator<GetTotalProductsInStockForStoreQuery>
{
    private IRepositoryManager _repositoryManager;
    public GetTotalProductsInStockForStoreQueryValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.StoreId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(StoreExists)
            .WithMessage("Store is not found.");
    }

    private async Task<bool> StoreExists(Guid storeId, CancellationToken cancellationToken)
        => await _repositoryManager.StoreRepository.StoreExistsAsync(storeId);
}