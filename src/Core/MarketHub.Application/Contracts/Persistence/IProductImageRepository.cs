using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IProductImageRepository
{
    Task<Guid> AddImageForProductAsync(Guid productId, ProductImage productImage);
}