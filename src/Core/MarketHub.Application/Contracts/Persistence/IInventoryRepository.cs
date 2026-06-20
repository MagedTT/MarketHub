using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IInventoryRepository
{
    Task AddAmountToProductAsync(Inventory inventory);
}