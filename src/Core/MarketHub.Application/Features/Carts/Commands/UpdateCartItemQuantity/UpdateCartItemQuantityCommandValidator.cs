using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Carts.Commands.UpdateCartItemQuantity;

public class UpdateCartItemQuantityCommandValidator : AbstractValidator<UpdateCartItemQuantityCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    public UpdateCartItemQuantityCommandValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.CartItemId)
            .NotEmpty()
            .WithMessage("{ProeprtyName} is Required.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("{ProeprtyName} is Required.")
            .MustAsync(UserExists)
            .WithMessage("User is not found.");

        RuleFor(x => x.CartId)
            .NotEmpty()
            .WithMessage("{ProeprtyName} is Required.")
            .MustAsync(CartExists)
            .WithMessage("Cart is not found.");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("{ProeprtyName} is Required.")
            .MustAsync(ProductExists)
            .WithMessage("Product is not found.");

        RuleFor(x => x.Quantity)
            .NotEmpty()
            .WithMessage("{ProeprtyName} is Required.")
            .GreaterThan(0)
            .WithMessage("Quantity should be greater than 0")
            .MustAsync(LessThanAvailableAmountInStock)
            .WithMessage("Amount in Stock is not Enough"); ;
    }

    private async Task<bool> UserExists(Guid userId, CancellationToken cancellationToken)
            => await _repositoryManager.UserRepository.CheckUserExistsAsync(userId);

    private async Task<bool> CartExists(Guid cartId, CancellationToken cancellationToken)
            => await _repositoryManager.CartRepository.CartExistsAsync(cartId);

    private async Task<bool> ProductExists(Guid productId, CancellationToken cancellationToken)
        => await _repositoryManager.ProductRepository.CheckProductExistsByIsAsync(productId);

    private async Task<bool> LessThanAvailableAmountInStock(UpdateCartItemQuantityCommand command, int quanitty, CancellationToken cancellationToken)
        => await _repositoryManager.InventoryRepository.CheckEnoughQuantityInStockAsync(command.ProductId, quanitty);
}