using MarketHub.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Persistence.Repositories;

public class StoreRepository : IStoreRepository
{
    private readonly MarketHubDbContext _context;

    public StoreRepository(MarketHubDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CheckStoreExistsAsync(Guid id)
    {
        return await _context.Stores.AnyAsync(store => store.Id.Equals(id));
    }
}