using System.ComponentModel;
using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Orders.Commands.MarkOrderAsDelivered;

public class MarkOrderAsDeliveredCommandValidator : AbstractValidator<MarkOrderAsDeliveredCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    public MarkOrderAsDeliveredCommandValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.");
    }

    private async Task<bool> OrderExists(Guid orderId, CancellationToken cancellationToken)
        => await _repositoryManager.OrdersRepository.OrderExistsByIdAsync(orderId);
}