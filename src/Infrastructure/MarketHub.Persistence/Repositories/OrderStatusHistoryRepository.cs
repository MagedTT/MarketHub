using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;

namespace MarketHub.Persistence.Repositories;

public class OrderStatusHistoryRepository : IOrderStatusHistoryRepository
{
    private readonly MarketHubDbContext _context;
    public OrderStatusHistoryRepository(MarketHubDbContext context)
        => _context = context;

    public void Create(OrderStatusHistory orderStatusHistory)
        => _context.OrderStatusHistories.Add(orderStatusHistory);

    public void Delete(OrderStatusHistory orderStatusHistory)
        => _context.OrderStatusHistories.Remove(orderStatusHistory);
}