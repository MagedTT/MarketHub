using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Persistence.Repositories;

public class PromoCodeRepository : IPromoCodeRepository
{
    private readonly MarketHubDbContext _context;
    public PromoCodeRepository(MarketHubDbContext context)
        => _context = context;

    public async Task<PromoCode?> PromoCodeExistsByCodeAsync(string code)
        => await _context.PromoCodes.FirstOrDefaultAsync(x => x.Code == code);
}