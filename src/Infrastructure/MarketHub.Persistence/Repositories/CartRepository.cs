using System.Security.Cryptography.X509Certificates;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Carts;
using MarketHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Persistence.Repositories;

public class CartRepository : ICartRepository
{
    private readonly MarketHubDbContext _context;
    public CartRepository(MarketHubDbContext context)
        => _context = context;

    public async Task<CartDto?> GetCartByUserIdAsync(Guid userId, bool trackChanges)
    {
        IQueryable<Cart> carts = _context.Carts;

        if (!trackChanges)
            carts = carts.AsNoTracking();

        CartDto? cartDto = await carts.Where(x => x.UserId == userId)
            .Select(x => new CartDto
            {
                CartId = x.Id,
                UserId = x.UserId,
                CreatedAt = x.CreatedAt,
                Items = x.Items.Select(item => new CartItemDto
                {
                    CartItemId = item.Id,
                    Quantity = item.Quantity,
                    SubTotal = item.Quantity * item.Product.Price,
                    Product = new CartProductDto
                    {
                        ProductId = item.ProductId,
                        ProductName = item.Product.Name,
                        ProductPrice = item.Product.Price,
                        BaseImageUrl = item.Product.Images.Select(image => image.ImageUrl).FirstOrDefault() ?? string.Empty
                    }
                }).ToList()
            })
            .FirstOrDefaultAsync();

        return cartDto;
    }

    public async Task<Guid?> CartExistsByUserIdAsync(Guid userId)
        => await _context.Carts.Where(x => x.UserId == userId).Select(cart => cart.Id).FirstOrDefaultAsync();


    public async Task<bool> CartExistsAsync(Guid userId)
        => await _context.Carts.AnyAsync(x => x.UserId == userId);
    public Task<Guid> CreateCartAsync(Cart cart)
    {
        _context.Carts.Add(cart);

        return Task.FromResult(cart.Id);
    }

    public async Task<bool> DeletCartForUserWithIdAsync(Guid userId)
    {
        if (await _context.Users.AnyAsync(user => user.Id == userId))
        {
            Cart? cart = await _context.Carts.FirstOrDefaultAsync(x => x.UserId == userId);

            if (cart is not null)
            {
                _context.Carts.Remove(cart);

                return true;
            }
        }

        return false;
    }

    public void DeleteCart(Cart cart)
        => _context.Carts.Remove(cart);
}