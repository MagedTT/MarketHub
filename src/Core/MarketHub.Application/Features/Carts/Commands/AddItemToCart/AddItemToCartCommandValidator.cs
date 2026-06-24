using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Carts.Commands.AddItemToCart;

public class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    public AddItemToCartCommandValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(UserExists)
            .WithMessage("User not found.");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(ProductExists)
            .WithMessage("Product not found.");

        RuleFor(x => x.Quantity)
            .NotEmpty()
            .WithMessage("Quantity is Required.")
            .GreaterThan(0)
            .WithMessage("Quantity should be greather than 0")
            .MustAsync(LessThanAvailableAmountInStock)
            .WithMessage("Amount in Stock is not Enough");
    }

    private async Task<bool> UserExists(Guid userId, CancellationToken cancellationToken)
        => await _repositoryManager.UserRepository.CheckUserExistsAsync(userId);

    private async Task<bool> ProductExists(Guid productId, CancellationToken cancellationToken)
        => await _repositoryManager.ProductRepository.CheckProductExistsByIsAsync(productId);

    private async Task<bool> LessThanAvailableAmountInStock(AddItemToCartCommand command, int quanitty, CancellationToken cancellationToken)
        => await _repositoryManager.InventoryRepository.CheckEnoughQuantityInStockAsync(command.ProductId, quanitty);
}