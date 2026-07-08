
using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Products.GetTopNBestSellingProductsForStore;

public class GetTopNBestSellingProductsForStoreQueryValidator : AbstractValidator<GetTopNBestSellingProductsForStoreQuery>
{
    private IRepositoryManager _repositoryManager;
    public GetTopNBestSellingProductsForStoreQueryValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.StoreId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(StoreExists)
            .WithMessage("Store is not found.");

        RuleFor(x => x.N)
            .GreaterThan(0)
            .WithMessage("Number of products should be greater than 0");
    }

    private async Task<bool> StoreExists(Guid storeId, CancellationToken cancellationToken)
        => await _repositoryManager.StoreRepository.StoreExistsAsync(storeId);
}