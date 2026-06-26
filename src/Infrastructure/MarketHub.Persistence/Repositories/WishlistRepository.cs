using System.Security.Cryptography.X509Certificates;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Wishlist;
using MarketHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Persistence.Repositories;

public class WishlistRepository : IWishlistReposiotry
{
    private readonly MarketHubDbContext _context;
    public WishlistRepository(MarketHubDbContext context)
        => _context = context;

    public async Task<Guid?> GetWishlistIdByUserIdAsync(Guid userId)
        => await _context.Wishlists.Where(x => x.UserId == userId).Select(wishlist => wishlist.Id).FirstOrDefaultAsync();

    public async Task<WishlistItem?> GetWishlistItemByProductIdIdAndWishlistIdAsync(Guid productId, Guid wishlistId)
        => await _context.WishlistItems.FirstOrDefaultAsync(x => x.ProductId == productId && x.WishlistId == wishlistId);

    public async Task<WishlistDto> GetWishlistWithItemsByUserIdAsync(Guid userId, bool trackChanges)
    {
        IQueryable<Wishlist> wishlists = _context.Wishlists;

        if (!trackChanges)
            wishlists = wishlists.AsNoTracking();

        WishlistDto wishlist = await wishlists.Where(x => x.UserId == userId).Select(x => new WishlistDto
        {
            Id = x.Id,
            UserId = x.UserId,
            WishlistItems = x.WishlistItems.Select(w => new WishlistItemDto
            {
                Id = w.Id,
                ProductId = w.ProductId,
                ProductName = w.Product.Name,
                ProductBaseImageUrl = w.Product.Images.Select(x => x.ImageUrl).FirstOrDefault() ?? string.Empty,
                ProductUnitPrice = w.Product.Price,
                ProductInStock = w.Product.Inventory.AvailableQuantity > 0
            }).ToList()
        }).SingleAsync();

        return wishlist;
    }

    public async Task<bool> WishlistContainsProductAsync(Guid wishlistId, Guid productId)
        => await _context.WishlistItems.AnyAsync(x => x.WishlistId == wishlistId && x.ProductId == productId);

    public async Task<bool> CheckWishlistExistsByUserIdAndWishlistIdAsync(Guid userId, Guid wishlistId)
        => await _context.Wishlists.AnyAsync(x => x.Id == wishlistId && x.UserId == userId);

    public async Task<bool> CheckWishlistExistsByUserIdAsync(Guid userId)
        => await _context.Wishlists.AnyAsync(x => x.UserId == userId);

    public void CreateWishlist(Wishlist wishlist)
        => _context.Wishlists.Add(wishlist);

    public void CreateWishlistItem(WishlistItem wishlistItem)
        => _context.WishlistItems.Add(wishlistItem);

    public void DeleteWishlist(Wishlist wishlist)
        => _context.Wishlists.Remove(wishlist);

    public void DeleteWishlistItem(WishlistItem wishlistItem)
        => _context.WishlistItems.Remove(wishlistItem);
}