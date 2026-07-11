using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly MarketHubDbContext _context;
    public CategoryRepository(MarketHubDbContext context)
        => _context = context;

    public async Task<IEnumerable<Category>> GetCategoriesAsync(bool trackChanges)
    {
        IQueryable<Category> categories = _context.Categories;

        if (!trackChanges)
            categories = categories.AsNoTracking();

        return await categories.ToListAsync();
    }
}