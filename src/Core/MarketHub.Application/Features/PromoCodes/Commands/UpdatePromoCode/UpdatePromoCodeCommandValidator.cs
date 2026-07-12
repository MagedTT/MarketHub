using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.PromoCodes.Commands.UpdatePromoCode;

public class UpdatePromoCodeCommandValidator : AbstractValidator<UpdatePromoCodeCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    public UpdatePromoCodeCommandValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.StoreId)
           .NotEmpty()
           .WithMessage("{PropertyName} is Required.")
           .MustAsync(StoreExists)
           .WithMessage("Store is not found.");

        RuleFor(x => x.PromoCodeId)
           .NotEmpty()
           .WithMessage("{PropertyName} is Required.")
           .MustAsync(StoreOwnsPromoCode)
           .WithMessage("You are not authorized.");
    }

    private async Task<bool> StoreExists(Guid storeId, CancellationToken cancellationToken)
        => await _repositoryManager.StoreRepository.StoreExistsAsync(storeId);

    private async Task<bool> StoreOwnsPromoCode(UpdatePromoCodeCommand command, Guid promoCodeId, CancellationToken cancellationToken)
        => await _repositoryManager.PromoCodeRepository.CheckPromoCodeExistsByIdAndOwnedByStoreAsync(promoCodeId, command.StoreId);
}