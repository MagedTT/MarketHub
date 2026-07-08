using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.PromoCodes.GetUsageCountForPromoCode;

public class GetUsageCountForPromoCodeQueryValidator : AbstractValidator<GetUsageCountForPromoCodeQuery>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetUsageCountForPromoCodeQueryValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.PromoCodeId)
                .NotEmpty()
                .WithMessage("{PropertyName} is Required.")
                .MustAsync(PromoCodeExists)
                .WithMessage("PromoCode is not found.");
    }

    private async Task<bool> PromoCodeExists(Guid promoCodeId, CancellationToken cancellationToken)
        => await _repositoryManager.PromoCodeRepository.CheckPromoCodeExistsByIdAsync(promoCodeId);
}