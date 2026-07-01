using MarketHub.Application.DTOs.Persistence.Orders;
using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;
using MarketHub.Domain.Enums;

namespace MarketHub.Application.Contracts.Persistence;

public interface IOrdersRepository
{
    Task<Order?> GetOrderByIdAsync(Guid orderId, bool trackChanges);
    Task<Order?> GetOrderByIdWithOrderItemsAsync(Guid orderId, bool trackChanges);
    Task<OrderDto?> GetOrderDtoByUserIdAndOrderIdAsync(Guid userId, Guid orderId, bool trackChanges);
    // Task<OrderDto?> GetOrderDtoByUserIdAndOrderIdAsync(Guid userId, Guid orderId, bool trackChanges);
    // Task<PagedList<OrderDto>> GetOrdersByUserIdAsync(Guid userId, bool trackChanges);
    // Task<PagedList<OrderDto>> GetOrdersByStatusAsync(OrderStatus orderStatus, bool trackChanges);
    Task<PagedList<OrderDto>> GetOrdersAsync(OrderParameters orderParameters, bool trackChanges);
    Task<int> OrdersCountByUserIdAsync(Guid userId);
    Task<decimal> TotalSpentsByUserIdAsync(Guid userId);
    Task<bool> OrderExistsByUserIdAndOrderIdAsync(Guid userId, Guid orderId);
    void CreateOrder(Order order);
    void DeleteOrder(Order order);
}