using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Persistence.Repositories;

public class StoreRepository : IStoreRepository
{
    private readonly MarketHubDbContext _context;

    public StoreRepository(MarketHubDbContext context)
        => _context = context;

    public async Task<Store?> GetByIdAsync(Guid storeId)
        => await _context.Stores.FirstOrDefaultAsync(x => x.Id == storeId);

    public async Task<bool> CheckStoreExistsAsync(Guid id)
        => await _context.Stores.AnyAsync(store => store.Id.Equals(id));

    public async Task<decimal> TotalSalesByStoreIdAsync(Guid storeId)
    => await _context.OrderItems.Where(x => x.Product.StoreId == storeId).SumAsync(x => x.UnitPrice * x.Quantity);

    public void CreateStore(Store store)
        => _context.Stores.Add(store);
}