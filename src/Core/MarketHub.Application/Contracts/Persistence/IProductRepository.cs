using MarketHub.Application.DTOs.Persistence.Product;
using MarketHub.Application.DTOs.Persistence.Review;
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
    Task<bool> CheckProductExistsByIsAsync(Guid productId);
    void UpdateProductAsync(Product product);
    Task MarkAsDeletedAsync(Guid id);


    /////// Store Product Methods ///////
    Task<int> TotalProductsByStoreIdAsync(Guid storeId);
    Task<int> TotalProductsInStockByStoreIdAsync(Guid storeId);
    Task<int> TotalProductsOutOfStockByStoreIdAsync(Guid storeId);
    Task<PagedList<ProductDto>> GetAllProductsByStoreIdAsync(Guid storeId, StoreProductsParameters storeProductsParameters);
    Task<IEnumerable<ProductDto>> TopNBestSellingProductsByStoreIdAsync(Guid storeId);
    Task<IEnumerable<StoreRatingCount>> RatingCountByStoreIdAsync(Guid storeId);
}