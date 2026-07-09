using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Orders;
using MarketHub.Application.Features.Carts.Queries.GetCartByUserId;
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

    public async Task<OrderDto?> GetOrderDtoByUserIdAndOrderIdAsync(Guid userId, Guid orderId, bool trackChanges)
    {
        IQueryable<Order> orders = _context.Orders;

        if (!trackChanges)
            orders = orders.AsNoTracking();

        return await orders
            .Where(x => x.Id == orderId && x.UserId == userId)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                OrderedByUserName = o.User.UserName!,
                NumberOfOrderedProducts = o.OrderItems.Count(),
                Status = o.Status,
                CreatedAt = o.CreatedAt,
                DateOfDelivery = o.CreatedAt.AddDays(3),
                // ShippingAddress = o.ShippingAddress,
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
                    ProductType = i.Product.Type,
                    ProductBaseImageUrl = i.Product.Images.Select(pi => pi.ImageUrl).FirstOrDefault() ?? string.Empty,
                    ProductSpecifications = JsonSerializer.Deserialize<JsonElement>(i.Product.Specifications)
                }).ToList()
            }).FirstOrDefaultAsync();
    }

    public async Task<Order?> GetOrderByIdWithOrderItemsAsync(Guid orderId, bool trackChanges)
    {
        IQueryable<Order> orders = _context.Orders;

        if (!trackChanges)
            orders = orders.AsNoTracking();

        return await orders.Include(x => x.OrderItems).FirstOrDefaultAsync(x => x.Id == orderId);
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
                OrderNumber = o.OrderNumber,
                CreatedAt = o.CreatedAt,
                DateOfDelivery = o.CreatedAt.AddDays(3),
                // ShippingAddress = o.ShippingAddress,
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
                    ProductType = i.Product.Type,
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

    /////// Store Product Methods ///////
    public async Task<int> TotalOrdersByStoreIdAsync(Guid storeId)
    {
        return await _context.OrderItems.Where(x => x.Product.StoreId == storeId)
            .Select(x => x.OrderId)
            .Distinct()
            .CountAsync();
    }

    public async Task<IEnumerable<StoreOrderStatusCount>> OrderStatusCountByStoreIdAsync(Guid storeId)
    {
        return await _context.OrderItems
            .Where(x => x.Product.StoreId == storeId)
            .Select(x => new
            {
                x.OrderId,
                x.Order.Status
            })
            .Distinct()
            .GroupBy(x => x.Status)
            .Select(x => new StoreOrderStatusCount
            {
                OrderStatus = x.Key,
                Count = x.Count()
            }).ToListAsync();
    }

    public async Task<PagedList<StoreOrderDto>> GetRecentOrdersByStoreIdAsync(Guid storeId, StoreOrdersParameters storeOrdersParameters)
    {
        IQueryable<Order> orders = _context.Orders.Where(x => x.OrderItems.Any(oi => oi.Product.StoreId == storeId));

        if (storeOrdersParameters.OrderStatus is not null)
            orders = orders.Where(x => x.Status == storeOrdersParameters.OrderStatus);

        int count = await orders.CountAsync();

        orders = orders.OrderByDescending(o => o.CreatedAt);

        List<StoreOrderDto> storeOrders = await orders
            .Select(x => new StoreOrderDto
            {
                OrderId = x.Id,
                StoreId = storeId,
                UserId = x.UserId,
                UserName = x.User.UserName!,
                PromoCode = x.PromoCode != null ? x.PromoCode.Code : null,
                OrderNumber = x.OrderNumber,
                Status = x.Status,
                TotalAmount = x.OrderItems.Where(x => x.Product.StoreId == storeId)
                    .Sum(oi => oi.Quantity * oi.UnitPrice),
                CreatedAt = x.CreatedAt,
                ShippingAddress = new StoreOrderShippingAddressDto
                {
                    Id = x.ShippingAddress.Id,
                    UserId = x.UserId,
                    FullName = x.User.UserName!,
                    PhoneNumber = x.ShippingAddress.PhoneNumber,
                    Country = x.ShippingAddress.Country,
                    Governorate = x.ShippingAddress.Governorate,
                    City = x.ShippingAddress.City,
                    Street = x.ShippingAddress.Street,
                    BuildingNumber = x.ShippingAddress.BuildingNumber,
                    Floor = x.ShippingAddress.Floor,
                    Apartment = x.ShippingAddress.Apartment,
                    PostalCode = x.ShippingAddress.PostalCode,
                    IsDefault = x.ShippingAddress.IsDefault
                },
                OrderItems = x.OrderItems
                    .Where(oi => oi.Product.StoreId == storeId)
                    .Select(oi => new StoreOrderItemDto
                    {
                        OrderItemId = oi.Id,
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.Name,
                        ProductBaseImageUrl = oi.Product.Images.Select(i => i.ImageUrl).FirstOrDefault() ?? string.Empty,
                        ProductQuantity = oi.Quantity,
                        ProductUnitPrice = oi.UnitPrice
                    }).ToList()
            })
            .Skip((storeOrdersParameters.PageNumber - 1) * storeOrdersParameters.PageSize)
            .Take(storeOrdersParameters.PageSize)
            .ToListAsync();

        return new PagedList<StoreOrderDto>(storeOrders, count, storeOrdersParameters.PageNumber, storeOrdersParameters.PageSize);
    }

    public async Task<StoreOrderDto?> GetOrderByOrderIdAndStoreIdAsync(Guid orderId, Guid storeId)
    {
        return await _context.Orders.Where(x => x.Id == orderId && x.OrderItems.Any(oi => oi.Product.StoreId == storeId)).Select(x => new StoreOrderDto
        {
            OrderId = x.Id,
            StoreId = storeId,
            UserId = x.UserId,
            UserName = x.User.UserName!,
            PromoCode = x.PromoCode != null ? x.PromoCode.Code : null,
            OrderNumber = x.OrderNumber,
            Status = x.Status,
            TotalAmount = x.OrderItems.Where(x => x.Product.StoreId == storeId)
                    .Sum(oi => oi.Quantity * oi.UnitPrice),
            CreatedAt = x.CreatedAt,
            ShippingAddress = new StoreOrderShippingAddressDto
            {
                Id = x.ShippingAddress.Id,
                UserId = x.UserId,
                FullName = x.User.UserName!,
                PhoneNumber = x.ShippingAddress.PhoneNumber,
                Country = x.ShippingAddress.Country,
                Governorate = x.ShippingAddress.Governorate,
                City = x.ShippingAddress.City,
                Street = x.ShippingAddress.Street,
                BuildingNumber = x.ShippingAddress.BuildingNumber,
                Floor = x.ShippingAddress.Floor,
                Apartment = x.ShippingAddress.Apartment,
                PostalCode = x.ShippingAddress.PostalCode,
                IsDefault = x.ShippingAddress.IsDefault
            },
            OrderItems = x.OrderItems
                    .Where(oi => oi.Product.StoreId == storeId)
                    .Select(oi => new StoreOrderItemDto
                    {
                        OrderItemId = oi.Id,
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.Name,
                        ProductBaseImageUrl = oi.Product.Images.Select(i => i.ImageUrl).FirstOrDefault() ?? string.Empty,
                        ProductQuantity = oi.Quantity,
                        ProductUnitPrice = oi.UnitPrice
                    }).ToList()
        }).FirstOrDefaultAsync();
    }

    public async Task<bool> OrderExistsByIdAsync(Guid orderId)
        => await _context.Orders.AnyAsync(x => x.Id == orderId);

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

