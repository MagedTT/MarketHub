using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Persistence.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly MarketHubDbContext _context;
    public InventoryRepository(MarketHubDbContext context)
    {
        _context = context;
    }

    public async Task<Inventory?> GetByIdAsync(Guid inventoryId, bool trackChanges)
    {
        IQueryable<Inventory> inventories = _context.Inventories;

        if (!trackChanges)
            inventories = inventories.AsNoTracking();

        return await inventories.FirstOrDefaultAsync(x => x.Id == inventoryId);
    }

    public async Task<Inventory?> GetByProductIdAsync(Guid productId, bool trackChanges)
    {
        IQueryable<Inventory> inventories = _context.Inventories;

        if (!trackChanges)
            inventories = inventories.AsNoTracking();

        return await _context.Inventories.FirstOrDefaultAsync(x => x.ProductId == productId);
    }

    public Task AddAmountToProductAsync(Inventory inventory)
        => Task.FromResult(_context.Inventories.Add(inventory));

    public async Task<bool> CheckEnoughQuantityInStockAsync(Guid productId, int quantity)
        => await _context.Inventories.AnyAsync(x => x.ProductId == productId && x.AvailableQuantity >= quantity);

    public void CreateInventory(Inventory inventory)
        => _context.Inventories.Add(inventory);

    public void DeleteInventory(Inventory inventory)
        => _context.Inventories.Remove(inventory);
}