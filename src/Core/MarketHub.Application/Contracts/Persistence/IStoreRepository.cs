using MarketHub.Application.Features.Stores.Queries.GetStoreIdAndStatus;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IStoreRepository
{
    Task<Store?> GetByIdAsync(Guid storeId);
    Task<StoreStatusDto?> GetStoreIdAndActiveStatusAsync(Guid userId);
    Task<bool> CheckStoreExistsAsync(Guid id);
    Task<Guid?> GetStoreUserIdForOrderAsync(Guid orderId);
    Task<Guid?> GetStoreUserIdForProductAsync(Guid productId);
    Task<decimal> TotalSalesByStoreIdAsync(Guid storeId);
    Task<bool> StoreExistsAsync(Guid storeId);
    void CreateStore(Store store);
}