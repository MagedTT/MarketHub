using System.Text.Json;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Orders;
using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;
using MarketHub.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Persistence.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly MarketHubDbContext _context;
    public OrdersRepository(MarketHubDbContext context)
        => _context = context;

    public async Task<Order?> GetOrderByIdAsync(Guid orderId, bool trackChanges)
    {
        IQueryable<Order> orders = _context.Orders;

        if (!trackChanges)
            orders = orders.AsNoTracking();

        return await orders.FirstOrDefaultAsync(x => x.Id == orderId);
    }

    public async Task<PagedList<OrderDto>> GetOrdersAsync(OrderParameters orderParameters, bool trackChanges)
    {
        IQueryable<Order> orders = _context.Orders;

        if (!trackChanges)
            orders = orders.AsNoTracking();

        if (orderParameters.UserId is not null)
            orders = orders.Where(x => x.UserId == orderParameters.UserId);

        if (orderParameters.OrderStatus is not null)
            orders = orders.Where(x => x.Status == orderParameters.OrderStatus);

        IQueryable<OrderDto> ordersDtos = orders
            .Where(x =>
                orderParameters.OrderMinTotalPrice <= x.TotalAmount && x.TotalAmount <= orderParameters.OrderMaxTotalPrice)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                OrderedByUserName = o.User.UserName!,
                NumberOfOrderedProducts = o.OrderItems.Count(),
                Status = o.Status,
                CreatedAt = o.CreatedAt,
                DateOfDelivery = o.CreatedAt.AddDays(3),
                ShippingAddress = o.ShippingAddress,
                TotalAmount = o.TotalAmount,
                PromoCode = o.PromoCode != null ? o.PromoCode.Code : null,
                Items = o.OrderItems.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    LineTotal = i.UnitPrice * i.Quantity,
                    ProductName = i.Product.Name,
                    ProductBaseImageUrl = i.Product.Images.Select(pi => pi.ImageUrl).FirstOrDefault() ?? string.Empty,
                    ProductSpecifications = JsonSerializer.Deserialize<JsonElement>(i.Product.Specifications)
                }).ToList()
            });

        if (orderParameters.OrderByCreationTimeDescending)
            ordersDtos = ordersDtos.OrderByDescending(x => x.CreatedAt);
        else
            ordersDtos = ordersDtos.OrderBy(x => x.CreatedAt);

        int count = await ordersDtos.CountAsync();

        List<OrderDto> ordersDtosList = await ordersDtos.Skip((orderParameters.PageNumber - 1) * orderParameters.PageSize).Take(orderParameters.PageSize).ToListAsync();

        return new PagedList<OrderDto>(ordersDtosList, count, orderParameters.PageNumber, orderParameters.PageSize);
    }

    public async Task<bool> OrderExistsByUserIdAndOrderIdAsync(Guid userId, Guid orderId)
        => await _context.Orders.AnyAsync(x => x.UserId == userId && x.Id == orderId);

    public async Task<int> OrdersCountByUserIdAsync(Guid userId)
        => await _context.Orders.CountAsync(x => x.UserId == userId);

    public async Task<decimal> TotalSpentsByUserIdAsync(Guid userId)
        => await _context.Orders
        .Where(
            x => x.UserId == userId &&
            (x.Status == OrderStatus.Shipped ||
            x.Status == OrderStatus.Confirmed ||
            x.Status == OrderStatus.Delivered))
        .SumAsync(x => x.TotalAmount);

    public void CreateOrder(Order order)
        => _context.Orders.Add(order);

    public void DeleteOrder(Order order)
        => _context.Orders.Remove(order);

    // public Task<PagedList<OrderDto>> GetAllOrdersAsync(bool trackChanges)
    // {
    //     throw new NotImplementedException();
    // }

    // public Task<PagedList<OrderDto>> GetOrdersByStatusAsync(OrderStatus orderStatus, bool trackChanges)
    // {
    //     throw new NotImplementedException();
    // }

    // public async Task<PagedList<OrderDto>> GetOrdersByUserIdAsync(Guid userId, bool trackChanges)
    // {
    //     throw new NotImplementedException();
    // }

    // public Task<OrderDto?> GetOrderDtoByUserIdAndOrderIdAsync(Guid userId, Guid orderId, bool trackChanges)
    // {
    //     throw new NotImplementedException();
    // }
}

