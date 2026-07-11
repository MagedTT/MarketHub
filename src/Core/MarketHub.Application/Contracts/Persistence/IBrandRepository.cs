using MarketHub.Application.DTOs.Persistence.Product;
using MarketHub.Application.Features.Brands.Queries.GetBrandsWithIdAndName;
using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IBrandRepository
{
    Task<Brand?> GetBrandByIdAsync(Guid id, bool trackChanges);
    Task<PagedList<Brand>> GetBrandsAsync(BrandParameters brandParameters, bool trackChanges);
    Task<PagedList<string>> GetBrandsNamesAsync(BrandParameters brandParameters, bool trackChanges);
    Task<IEnumerable<BrandDto>> GetBrandsWithIdAndName(bool trackChanges);

    void AddBrand(Brand brand);
    Task<bool> CheckBrandExistsAsync(Guid id);
    Task<bool> BrandExistsByNameAsync(string brandName);

    void DeleteBrand(Brand brand);
    Task DeleteBrandAsync(Guid id);

    /////// Store Product Methods ///////
    Task<int> TotalBrandsByStoreIdAsync(Guid storeId);
    Task<IEnumerable<TopBrandDto>> TopNBestSellingBrandsByStoreIdAsync(Guid storeId, int n);
}