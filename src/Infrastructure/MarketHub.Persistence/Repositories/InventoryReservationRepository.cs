using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;

namespace MarketHub.Persistence.Repositories;

public class InventoryReservationRepository : IInventoryReservationRepository
{
    private readonly MarketHubDbContext _context;
    public InventoryReservationRepository(MarketHubDbContext context)
        => _context = context;

    public void CreateInventoryReservation(InventoryReservation inventoryReservation)
    {
        throw new NotImplementedException();
    }

    public Task<InventoryReservation?> GetActiveReservationAsync(Guid userId, Guid productId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<InventoryReservation>> GetActiveReservationsAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}