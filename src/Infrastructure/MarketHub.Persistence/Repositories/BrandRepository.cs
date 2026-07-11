using System.Security.Cryptography.X509Certificates;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Product;
using MarketHub.Application.Features.Brands.Queries.GetBrandsWithIdAndName;
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

    /////// Store Product Methods ///////
    public async Task<int> TotalBrandsByStoreIdAsync(Guid storeId)
    {
        return await _context.Products
            .Where(x => x.StoreId == storeId && x.BrandId != null)
            .Select(x => x.BrandId)
            .Distinct()
            .CountAsync();
    }

    public async Task<IEnumerable<TopBrandDto>> TopNBestSellingBrandsByStoreIdAsync(Guid storeId, int n)
    {
        return await _context.Products
            .Where(x => x.StoreId == storeId && x.BrandId != null)
            .GroupBy(x => new
            {
                x.BrandId,
                x.Brand!.Name
            })
            .Select(x => new TopBrandDto
            {
                BrandId = x.Key!.BrandId!.Value,
                Name = x.Key.Name,
                TotalSoldPieces = x.Sum(p => p.NumberOfSoldPieces)
            })
            .OrderByDescending(x => x.TotalSoldPieces)
            .Take(n)
            .ToListAsync();
    }

    public async Task<IEnumerable<BrandDto>> GetBrandsWithIdAndName(bool trackChanges)
    {
        IQueryable<Brand> brands = _context.Brands;

        if (!trackChanges)
            brands = brands.AsNoTracking();

        return await brands.Select(x => new BrandDto
        {
            BrandId = x.Id,
            Name = x.Name
        }).ToListAsync();
    }
}