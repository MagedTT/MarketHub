using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Orders.Commands.CancelOrder;

public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    private IRepositoryManager _repositoryManager;
    public CancelOrderCommandValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(UserExists)
            .WithMessage("User not found");

        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(OrderExists)
            .WithMessage("Order not found.");
    }

    private async Task<bool> UserExists(Guid userId, CancellationToken cancellationToken)
        => await _repositoryManager.UserRepository.CheckUserExistsAsync(userId);

    private async Task<bool> OrderExists(CancelOrderCommand command, Guid orderId, CancellationToken cancellationToken)
        => await _repositoryManager.OrdersRepository.OrderExistsByUserIdAndOrderIdAsync(command.UserId, orderId);
}