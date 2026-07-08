using FluentValidation;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Product;

namespace MarketHub.Application.Features.Brands.Queries.GetTotalBrandsForStore;

public class GetTopNSellingBrandsForStoreQueryValidator : AbstractValidator<GetTopNSellingBrandsForStoreQuery>
{
    private IRepositoryManager _repositoryManager;
    public GetTopNSellingBrandsForStoreQueryValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.StoreId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(StoreExists)
            .WithMessage("Store is not found.");

        RuleFor(x => x.N)
            .GreaterThan(0)
            .WithMessage("Number of Brands should be greater than 0");
    }

    private async Task<bool> StoreExists(Guid storeId, CancellationToken cancellationToken)
        => await _repositoryManager.StoreRepository.StoreExistsAsync(storeId);
}