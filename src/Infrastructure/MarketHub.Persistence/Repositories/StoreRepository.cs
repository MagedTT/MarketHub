using System.Security.Cryptography.X509Certificates;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;
using MarketHub.Domain.Enums;
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
        => await _context.OrderItems
            .Where(x => x.Product.StoreId == storeId && x.Order.Status != OrderStatus.Cancelled)
            .SumAsync(x => x.Order.PromoCode != null && x.Order.PromoCode.DiscountType == DiscountType.Percentage ? x.UnitPrice * x.Quantity * (1 - x.Order.PromoCode.DiscountValue / 100m) : x.UnitPrice * x.Quantity);

    public async Task<bool> StoreExistsAsync(Guid storeId)
        => await _context.Stores.AnyAsync(x => x.Id == storeId);

    public void CreateStore(Store store)
        => _context.Stores.Add(store);
}