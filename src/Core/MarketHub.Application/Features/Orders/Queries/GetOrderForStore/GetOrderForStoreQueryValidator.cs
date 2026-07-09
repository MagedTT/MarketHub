using FluentValidation;
using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Orders.Queries.GetOrderForStore;

public class GetOrderForStoreQueryValidator : AbstractValidator<GetOrderForStoreQuery>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetOrderForStoreQueryValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.StoreId)
           .NotEmpty()
           .WithMessage("{PropertyName} is Required.")
           .MustAsync(StoreExists)
           .WithMessage("Store is not found.");

        RuleFor(x => x.OrderId)
           .NotEmpty()
           .WithMessage("{PropertyName} is Required.")
           .MustAsync(OrderExists)
           .WithMessage("Order is not found.");
    }

    private async Task<bool> StoreExists(Guid storeId, CancellationToken cancellationToken)
        => await _repositoryManager.StoreRepository.StoreExistsAsync(storeId);

    private async Task<bool> OrderExists(Guid orderId, CancellationToken cancellationToken)
        => await _repositoryManager.OrdersRepository.OrderExistsByIdAsync(orderId);
}