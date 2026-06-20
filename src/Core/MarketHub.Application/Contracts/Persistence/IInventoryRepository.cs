using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IInventoryRepository
{
    void AddAmountToProductAsync(Inventory inventory);
}