using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IStoreRepository
{
    Task<Store?> GetByIdAsync(Guid storeId);
    Task<Guid> GetStoreIdAsync(Guid userId);
    Task<bool> CheckStoreExistsAsync(Guid id);
    Task<decimal> TotalSalesByStoreIdAsync(Guid storeId);
    Task<bool> StoreExistsAsync(Guid storeId);
    void CreateStore(Store store);
}