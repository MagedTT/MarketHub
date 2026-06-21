using MarketHub.Application.DTOs.Persistence.Carts;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface ICartItemRepository
{
    Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(Guid cartId); // when user leaves the cart page the cartId, and its cartItems Ids is sent to the server then you call this function and check for each if it exists and if not remove from db and if yes then update quantity and check if quantity in stock is enough
    void AddCartItem(CartItem cartItem);
    void UpdateCartItemQuantity(CartItem cartItem);
}