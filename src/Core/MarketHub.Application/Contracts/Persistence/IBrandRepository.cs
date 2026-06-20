using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IBrandRepository
{
    Task<Brand> GetBrandByIdAsync(Guid id, bool trackChanges);
    Task<PagedList<Brand>> GetBrandsAsync(BrandParameters brandParameters, bool trackChanges);
    Task<PagedList<string>> GetBrandsNamesAsync(BrandParameters brandParameters, bool trackChanges);

    Task AddBrandAsync(Brand brand);
    Task<bool> CheckBrandExistsAsync(Guid id);

    Task DeleteBrandAsync(Brand brand);
    Task DeleteBrandAsync(Guid id);
}