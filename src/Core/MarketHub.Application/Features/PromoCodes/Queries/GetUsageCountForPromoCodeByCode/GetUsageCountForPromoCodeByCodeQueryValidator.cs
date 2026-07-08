using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.PromoCodes.GetUsageCountForPromoCodeByCode;

public class GetUsageCountForPromoCodeByCodeQueryValidator : AbstractValidator<GetUsageCountForPromoCodeByCodeQuery>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetUsageCountForPromoCodeByCodeQueryValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("{PropertyName} is Required.")
                .MustAsync(PromoCodeExists)
                .WithMessage("PromoCode is not found.");
    }

    private async Task<bool> PromoCodeExists(string code, CancellationToken cancellationToken)
        => await _repositoryManager.PromoCodeRepository.CheckPromoCodeExistsByCodeAsync(code);
}