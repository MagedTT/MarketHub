namespace MarketHub.Application.Contracts.Persistence;

public interface IRepositoryManager
{
    IBrandRepository BrandRepository { get; }
    IStoreRepository StoreRepository { get; }
    IProductRepository ProductRepository { get; }
    IProductImageRepository ProductImageRepository { get; }
    IInventoryRepository InventoryRepository { get; }
    ICartRepository CartRepository { get; }
    ICartItemRepository CartItemRepository { get; }
    IUserRepository UserRepository { get; }
    IWishlistReposiotry WishlistRepository { get; }
    IReviewRepository ReviewRepository { get; }
    Task<int> SaveAsync();
}