namespace MarketHub.Application.Contracts.Persistence;

public interface IRepositoryManager
{
    IBrandRepository BrandRepository { get; }
    IStoreRepository StoreRepository { get; }
    IProductRepository ProductRepository { get; }
    IProductImageRepository ProductImageRepository { get; }
    InventoryRepository InventoryRepository { get; }
    Task<int> SaveAsync();
}