using MarketHub.Application.DTOs.Persistence.Carts;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface ICartRepository
{
    Task<Guid> CreateCartAsync(Cart cart);
    Task<CartDto?> GetCartByUserIdAsync(Guid userId, bool trackChanges);
    Task<Guid?> CartExistsByUserIdAsync(Guid userId);
    Task<bool> DeletCartForUserWithIdAsync(Guid userId);
    void DeleteCart(Cart cart);
}