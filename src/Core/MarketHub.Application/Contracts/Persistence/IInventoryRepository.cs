using MarketHub.Application.DTOs.Persistence.Inventories;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IInventoryRepository
{
    Task<Inventory?> GetByProductIdAsync(Guid productId, bool trackChanges);
    Task<InventoryDto?> GetInventoryDtoByProductIdAsync(Guid productId, bool trackChanges);
    Task<Inventory?> GetByIdAsync(Guid inventoryId, bool trackChanges);
    Task AddAmountToProductAsync(Inventory inventory);
    Task<bool> InvenotryExistsByProductIdAsync(Guid productId);
    Task<bool> CheckEnoughQuantityInStockAsync(Guid productId, int quantity);
    void CreateInventory(Inventory inventory);
    void DeleteInventory(Inventory inventory);
}