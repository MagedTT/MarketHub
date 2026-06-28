using MarketHub.Application.Contracts.Infrastructure;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;

namespace MarketHub.Infrastructure;

public class InventoryService : IInventoryService
{
    private readonly IRepositoryManager _repositoryManager;
    public InventoryService(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<(bool Success, string Message)> ReserveStockAsync(Guid productId, int quantity)
    {
        Inventory? inventory = await _repositoryManager.InventoryRepository.GetByProductIdAsync(productId, trackChanges: true);

        if (inventory is null)
            return (false, $"Inventory for product with Id: {productId} is not found");

        if (inventory.AvailableQuantity < quantity)
            return (false, "No enough available quantity in stock");

        inventory.AvailableQuantity -= quantity;
        inventory.ReservedQuantity += quantity;

        return (true, "reserved");
    }

    public async Task<(bool Success, string Message)> ReleaseReservedStockAsync(Guid productId, int quantity)
    {
        Inventory? inventory = await _repositoryManager.InventoryRepository.GetByProductIdAsync(productId, trackChanges: true);

        if (inventory is null)
            return (false, $"Inventory for product with Id: {productId} is not found");

        inventory.AvailableQuantity += quantity;
        inventory.ReservedQuantity -= quantity;

        return (true, "released");
    }

    public async Task<(bool Success, string Message)> ConfirmReservedStockAsync(Guid productId, int quantity)
    {
        Inventory? inventory = await _repositoryManager.InventoryRepository.GetByProductIdAsync(productId, trackChanges: true);

        if (inventory is null)
            return (false, $"Inventory for product with Id: {productId} is not found");

        inventory.ReservedQuantity -= quantity;

        return (true, "confirmed");
    }
}