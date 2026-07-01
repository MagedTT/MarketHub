using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Persistence.Repositories;

public class InventoryReservationRepository : IInventoryReservationRepository
{
    private readonly MarketHubDbContext _context;
    public InventoryReservationRepository(MarketHubDbContext context)
        => _context = context;

    public async Task<IEnumerable<InventoryReservation>> GetReservationsByUserIdAndProductIdsAsync(Guid userId, HashSet<Guid> productIds)
        => await _context.InventoryReservations.Where(x => x.UserId == userId && productIds.Contains(x.ProductId)).ToListAsync();

    public async Task<InventoryReservation?> GetActiveReservationAsync(Guid userId, Guid productId)
        => await _context.InventoryReservations
            .Include(x => x.Inventory)
            .FirstOrDefaultAsync(x =>
                x.Status == Domain.Enums.InventoryReservationStatus.Active &&
                x.ExpiresAt > DateTime.Now &&
                x.UserId == userId &&
                x.ProductId == productId);

    public async Task<IEnumerable<InventoryReservation>> GetActiveReservationsAsync(Guid userId)
        => await _context.InventoryReservations
            .Include(x => x.Inventory)
            .Where(x =>
                x.Status == Domain.Enums.InventoryReservationStatus.Active &&
                x.ExpiresAt > DateTime.Now &&
                x.UserId == userId)
            .ToListAsync();

    public void CreateInventoryReservation(InventoryReservation inventoryReservation)
        => _context.InventoryReservations.Add(inventoryReservation);
}