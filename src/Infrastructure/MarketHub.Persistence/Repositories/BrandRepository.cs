using System.Diagnostics.CodeAnalysis;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Persistence.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly MarketHubDbContext _context;

    public BrandRepository(MarketHubDbContext context)
        => _context = context;

    public async Task<Brand?> GetBrandByIdAsync(Guid id, bool trackChanges)
    {
        IQueryable<Brand> brands = _context.Brands;

        if (!trackChanges)
            brands = brands.AsNoTracking();

        return await brands.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PagedList<Brand>> GetBrandsAsync(BrandParameters brandParameters, bool trackChanges)
    {
        IQueryable<Brand> brands = _context.Brands;

        if (!trackChanges)
            brands = brands.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(brandParameters.SearchBrandName))
            brands = brands.Where(brand => brand.Name.Contains(brandParameters.SearchBrandName));

        int count = await brands.CountAsync();

        List<Brand> brandsList = await brands
            .OrderBy(x => x.Name)
            .Skip((brandParameters.PageNumber - 1) * brandParameters.PageSize)
            .Take(brandParameters.PageSize)
            .ToListAsync();

        return new PagedList<Brand>(brandsList, count, brandParameters.PageNumber, brandParameters.PageSize);
    }

    public async Task<PagedList<string>> GetBrandsNamesAsync(BrandParameters brandParameters, bool trackChanges)
    {
        IQueryable<Brand> brands = _context.Brands;

        if (!trackChanges)
            brands = brands.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(brandParameters.SearchBrandName))
            brands = brands.Where(brand => brand.Name.Contains(brandParameters.SearchBrandName));

        int count = await brands.CountAsync();

        List<string> brandNames = await brands
            .OrderBy(brand => brand.Name)
            .Skip((brandParameters.PageNumber - 1) * brandParameters.PageSize)
            .Take(brandParameters.PageSize)
            .Select(brand => brand.Name)
            .ToListAsync();

        return new PagedList<string>(brandNames, count, brandParameters.PageNumber, brandParameters.PageSize);
    }

    public async Task<bool> CheckBrandExistsAsync(Guid id)
        => await _context.Brands.AnyAsync(brand => brand.Id.Equals(id));

    public async Task<bool> BrandExistsByNameAsync(string brandName)
        => await _context.Brands.AnyAsync(brand => brand.Name == brandName);

    public void AddBrand(Brand brand)
        => _context.Brands.Add(brand);

    public void DeleteBrand(Brand brand)
        => _context.Brands.Remove(brand);

    public async Task DeleteBrandAsync(Guid id)
    {
        Brand? brand = await _context.Brands.FindAsync(id);

        if (brand is not null)
            _context.Brands.Remove(brand);
    }
}