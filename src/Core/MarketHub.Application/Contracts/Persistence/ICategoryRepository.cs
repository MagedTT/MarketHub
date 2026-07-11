using MarketHub.Application.DTOs.Persistence.Product;
using MarketHub.Application.Features.Brands.Queries.GetBrandsWithIdAndName;
using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetCategoriesAsync(bool trackChanges);
}