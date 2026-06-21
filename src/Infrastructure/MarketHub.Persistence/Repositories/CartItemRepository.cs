using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Persistence.Repositories;

public class CartItemRepository : ICartItemRepository
{
    private readonly MarketHubDbContext _context;
    public CartItemRepository(MarketHubDbContext context)
        => _context = context;

    public async Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(Guid cartId)
        => await _context.CartItems.Where(cartItem => cartItem.CartId == cartId).ToListAsync();

    public void AddCartItem(CartItem cartItem)
        => _context.CartItems.Add(cartItem);

    public void UpdateCartItemQuantity(CartItem cartItem)
        => _context.CartItems.Update(cartItem);
}