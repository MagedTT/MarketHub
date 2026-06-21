using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IInventoryRepository
{
    Task AddAmountToProductAsync(Inventory inventory);
    Task<bool> CheckEnoughQuantityInStockAsync(Guid productId, int quantity);
}