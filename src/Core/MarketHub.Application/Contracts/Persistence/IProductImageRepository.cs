using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IProductImageRepository
{
    Task AddImageForProductAsync(ProductImage productImage);
}