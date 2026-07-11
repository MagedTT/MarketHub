using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Products.Commands.DeactivateProductCommand;

public class ActivateProductCommandValidator : AbstractValidator<ActivateProductCommand>
{
    private IRepositoryManager _repositoryManager;
    public ActivateProductCommandValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.StoreId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(StoreExists)
            .WithMessage("Store is not found.");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(ProductExists)
            .WithMessage("Product is not found.")
            .MustAsync(StoreOwnsProduct)
            .WithMessage("You are not authorized to edit the product");
    }

    private async Task<bool> StoreExists(Guid storeId, CancellationToken cancellationToken)
        => await _repositoryManager.StoreRepository.StoreExistsAsync(storeId);

    private async Task<bool> ProductExists(Guid productId, CancellationToken cancellationToken)
        => await _repositoryManager.ProductRepository.CheckProductExistsForStoreByIsAsync(productId);

    private async Task<bool> StoreOwnsProduct(ActivateProductCommand command, Guid productId, CancellationToken cancellationToken)
        => await _repositoryManager.ProductRepository.StoreOwnsProductAsync(command.StoreId, productId);
}