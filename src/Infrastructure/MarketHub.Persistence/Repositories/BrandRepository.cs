using MarketHub.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Persistence.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly MarketHubDbContext _context;

    public BrandRepository(MarketHubDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CheckBrandExistsAsync(Guid id)
    {
        return await _context.Brands.AnyAsync(brand => brand.Id.Equals(id));
    }
}