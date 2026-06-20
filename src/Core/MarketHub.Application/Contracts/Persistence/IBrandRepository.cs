using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IBrandRepository
{
    Task<Brand?> GetBrandByIdAsync(Guid id, bool trackChanges);
    Task<PagedList<Brand>> GetBrandsAsync(BrandParameters brandParameters, bool trackChanges);
    Task<PagedList<string>> GetBrandsNamesAsync(BrandParameters brandParameters, bool trackChanges);

    void AddBrand(Brand brand);
    Task<bool> CheckBrandExistsAsync(Guid id);

    void DeleteBrand(Brand brand);
    Task DeleteBrandAsync(Guid id);
}