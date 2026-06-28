namespace MarketHub.Application.Contracts.Infrastructure;

public interface IInventoryService
{
    Task<(bool Success, string Message)> ReserveStockAsync(Guid productId, int quantity);
    Task<(bool Success, string Message)> ReleaseReservedStockAsync(Guid productId, int quantity);
    Task<(bool Success, string Message)> ConfirmReservedStockAsync(Guid productId, int quantity);
}