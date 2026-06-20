using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;

namespace MarketHub.Persistence.Repositories;

public class ProductImageRepository : IProductImageRepository
{
    private readonly MarketHubDbContext _context;
    public ProductImageRepository(MarketHubDbContext context)
    {
        _context = context;
    }

    public void AddImageForProductAsync(ProductImage productImage)
        => _context.ProductImages.Add(productImage);
}