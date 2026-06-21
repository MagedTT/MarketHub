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

    public Task AddAmountToProductAsync(Inventory inventory)
        => Task.FromResult(_context.Inventories.Add(inventory));

    public async Task<bool> CheckEnoughQuantityInStockAsync(Guid productId, int quantity)
        => await _context.Inventories.AnyAsync(x => x.ProductId == productId && x.AvailableQuantity >= quantity);
}