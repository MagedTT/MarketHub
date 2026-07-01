using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IInventoryReservationRepository
{
    Task<InventoryReservation?> GetActiveReservationAsync(Guid userId, Guid productId);
    Task<IEnumerable<InventoryReservation>> GetActiveReservationsAsync(Guid userId);
    void CreateInventoryReservation(InventoryReservation inventoryReservation);
}