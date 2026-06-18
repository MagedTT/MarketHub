using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface InventoryRepository
{
    Task AddAmountToProductAsync(Guid productId, Inventory inventory);
}