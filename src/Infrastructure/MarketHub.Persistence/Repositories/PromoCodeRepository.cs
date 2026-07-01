using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;

namespace MarketHub.Persistence.Repositories;

public class PromoCodeRepository : IPromoCodeRepository
{
    private readonly MarketHubDbContext _context;
    public PromoCodeRepository(MarketHubDbContext context)
        => _context = context;

    public Task<PromoCode?> PromoCodeExistsByCodeAsync(string code)
    {
        throw new NotImplementedException();
    }
}