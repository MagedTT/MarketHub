using FluentValidation;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Carts.Commands.RemoveCartItem;

public class RemoveCartItemCommandValidator : AbstractValidator<RemoveCartItemCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    public RemoveCartItemCommandValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(UserExists)
            .WithMessage("User is not found.");

        RuleFor(x => x.CartId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(CartExists)
            .WithMessage("Cart is not found.");

        RuleFor(x => x.CartItemId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.");
    }

    private async Task<bool> UserExists(Guid userId, CancellationToken cancellationToken)
            => await _repositoryManager.UserRepository.CheckUserExistsAsync(userId);

    private async Task<bool> CartExists(Guid cartId, CancellationToken cancellationToken)
        => await _repositoryManager.CartRepository.CartExistsByIdAsync(cartId);
}