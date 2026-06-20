using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;

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
}