using System.Linq.Expressions;
using MarketHub.Application.DTOs.Persistence.Product;
using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IProductRepository
{
    Task<ProductCardDto?> GetProductCardByIdAsync(Guid id, bool trackChanges);
    Task<ProductDetailsDto?> GetProductDetailsByIdAsync(Guid id, bool trackChanges);
    Task<PagedList<ProductCardDto>> GetProductCardsAsync(ProductParameters productParameters, bool trackChanges);
    // Task<PagedList<ProductCardDto>> GetProductCardsByConditionAsync(Expression<Func<Product, bool>> expression, bool trackChanges);
    Task<Guid> AddProductAsync(Product product);
    void UpdateProductAsync(Product product);
    Task MarkAsDeletedAsync(Guid id);
}