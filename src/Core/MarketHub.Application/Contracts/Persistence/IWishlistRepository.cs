using MarketHub.Application.DTOs.Persistence.Wishlist;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IWishlistReposiotry
{
    Task<IEnumerable<WishlistDto>> GetWishlistWithItemsByUserIdAsync(Guid userId, bool trackChanges);
    Task<Guid?> GetWishlistIdByUserIddAsync(Guid userId);
    Task<bool> WishlistContainsProductAsync(Guid wishlistId, Guid productId);
    void CreateWishlist(Wishlist wishlist);
    void DeleteWishlist(Wishlist wishlist);
    void CreateWishlistItem(WishlistItem wishlistItem);
    void DeleteWishlistItem(WishlistItem wishlistItem);
}