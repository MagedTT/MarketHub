using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    private IRepositoryManager _repositoryManager;
    public CreateOrderCommandValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(UserExists)
            .WithMessage("User not found");

        RuleFor(x => x.ShippingAddress)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MaximumLength(500)
            .WithMessage("{PropertyName} maximum number of characters is 500 character.");
    }

    private async Task<bool> UserExists(Guid userId, CancellationToken cancellationToken)
        => await _repositoryManager.UserRepository.CheckUserExistsAsync(userId);

    private async Task<bool> ProductExists(Guid productId, CancellationToken cancellationToken)
        => await _repositoryManager.ProductRepository.CheckProductExistsByIsAsync(productId);
}