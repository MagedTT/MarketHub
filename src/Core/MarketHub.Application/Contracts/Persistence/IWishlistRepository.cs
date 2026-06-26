using MarketHub.Application.DTOs.Persistence.Wishlist;
using MarketHub.Application.Features.Carts.Queries.GetCartByUserId;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IWishlistReposiotry
{
    Task<WishlistDto> GetWishlistWithItemsByUserIdAsync(Guid userId, bool trackChanges);
    Task<Guid?> GetWishlistIdByUserIdAsync(Guid userId);
    Task<WishlistItem?> GetWishlistItemByProductIdIdAndWishlistIdAsync(Guid productId, Guid wishlistId);
    Task<bool> WishlistContainsProductAsync(Guid wishlistId, Guid productId);
    Task<bool> CheckWishlistExistsByUserIdAsync(Guid userId);
    Task<bool> CheckWishlistExistsByUserIdAndWishlistIdAsync(Guid userId, Guid wishlistId);
    void CreateWishlist(Wishlist wishlist);
    void DeleteWishlist(Wishlist wishlist);
    void CreateWishlistItem(WishlistItem wishlistItem);
    void DeleteWishlistItem(WishlistItem wishlistItem);
}