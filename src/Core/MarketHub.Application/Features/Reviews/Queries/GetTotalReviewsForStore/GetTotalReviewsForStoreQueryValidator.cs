using FluentValidation;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Reviews.Queries.GetTotalReviewsForStore;

public class GetTotalReviewsForStoreQueryValidator : AbstractValidator<GetTotalReviewsForStoreQuery>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetTotalReviewsForStoreQueryValidator(IRepositoryManager repositoryManager)
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