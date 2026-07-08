using FluentValidation;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Enums;

namespace MarketHub.Application.Features.PromoCodes.Commands.CreatePromoCode;

public class CreatePromoCodeCommandValidator : AbstractValidator<CreatePromoCodeCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    public CreatePromoCodeCommandValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.StoreId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(StoreExists)
            .WithMessage("Store is not found.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .Length(8)
            .WithMessage("{PropertyName} should have exactly 8 characters")
            .Matches("^[a-zA-Z0-9]*$")
            .WithMessage("{PropertyName} must contain only English characters and digits.")
            .MustAsync(CodeUnique)
            .WithMessage("{PropertyName} already exists");

        RuleFor(x => x.DiscountType)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(DiscountTypeMatchesWithDiscountValue);

        RuleFor(x => x.DiscountValue)
            .GreaterThan(0)
            .WithMessage("{PropertyName} should be greater than 0");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(GreaterThanStartDate)
            .WithMessage("{PropertyName} should be greater than StartDate");

        RuleFor(x => x.UsageLimit)
            .GreaterThan(0)
            .WithMessage("{PropertyName} should be greater than 0");
    }

    private async Task<bool> StoreExists(Guid storeId, CancellationToken cancellationToken)
        => await _repositoryManager.StoreRepository.StoreExistsAsync(storeId);

    private async Task<bool> CodeUnique(string code, CancellationToken cancellationToken)
        => await _repositoryManager.PromoCodeRepository.CheckPromoCodeUniqueByCodeAsync(code);

    private async Task<bool> DiscountTypeMatchesWithDiscountValue(CreatePromoCodeCommand command, DiscountType discountType, CancellationToken cancellationToken)
    {
        if (discountType == DiscountType.Percentage)
        {
            return 1 <= command.DiscountValue && command.DiscountValue <= 100;
        }

        return true;
    }

    private async Task<bool> GreaterThanStartDate(DateTime endDate, CancellationToken cancellationToken)
        => DateTime.Now.Day < endDate.Day;
}