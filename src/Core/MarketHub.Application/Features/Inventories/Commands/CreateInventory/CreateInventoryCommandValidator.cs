using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Inventories.COmmands.CreateInventory;

public class CreateInventoryCommandValidator : AbstractValidator<CreateInventoryCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    public CreateInventoryCommandValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("{PropertyName} Is Required.")
            .MustAsync(ProductExists)
            .WithMessage("Product not found.");

        RuleFor(x => x.AvailableQuantity)
            .NotEmpty()
            .WithMessage("{PropertyName} Is Required.")
            .GreaterThan(0)
            .WithMessage("{PropertyName} should be greater than 0");
    }

    private async Task<bool> ProductExists(Guid productId, CancellationToken cancellationToken)
        => await _repositoryManager.ProductRepository.CheckProductExistsByIsAsync(productId);
}