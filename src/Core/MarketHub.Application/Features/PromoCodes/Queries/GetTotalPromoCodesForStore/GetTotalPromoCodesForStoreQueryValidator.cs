using FluentValidation;
using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Queries.GetTotalPromoCodesForStore;

public class GetTotalPromoCodesForStoreQueryValidator : AbstractValidator<GetTotalPromoCodesForStoreQuery>
{
    private readonly IRepositoryManager _repositoryManager;

    public GetTotalPromoCodesForStoreQueryValidator(IRepositoryManager repositoryManager)
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