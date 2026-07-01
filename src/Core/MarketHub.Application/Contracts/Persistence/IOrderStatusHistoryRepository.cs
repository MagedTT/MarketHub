using MarketHub.Application.DTOs.Persistence.Orders;
using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;
using MarketHub.Domain.Enums;

namespace MarketHub.Application.Contracts.Persistence;

public interface IOrderStatusHistoryRepository
{
    void Create(OrderStatusHistory orderStatusHistory);
    void Delete(OrderStatusHistory orderStatusHistory);
}