using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IStoreRepository
{
    Task<Store?> GetByIdAsync(Guid storeId);
    Task<bool> CheckStoreExistsAsync(Guid id);
    Task<decimal> TotalSalesByStoreIdAsync(Guid storeId);
    void CreateStore(Store store);
}