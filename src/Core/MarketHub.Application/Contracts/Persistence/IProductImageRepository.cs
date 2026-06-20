using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IProductImageRepository
{
    void AddImageForProductAsync(ProductImage productImage);
}