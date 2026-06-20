using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;

namespace MarketHub.Persistence.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly MarketHubDbContext _context;
    private readonly Lazy<IBrandRepository> _brandRepository;
    private readonly Lazy<IStoreRepository> _storeRepository;
    private readonly Lazy<IProductRepository> _productRepository;
    private readonly Lazy<IProductImageRepository> _productImageRepository;
    private readonly Lazy<IInventoryRepository> _inventoryRepository;
    public RepositoryManager(MarketHubDbContext context)
    {
        _context = context;
        _brandRepository = new Lazy<IBrandRepository>(() => new BrandRepository(context));
        _storeRepository = new Lazy<IStoreRepository>(() => new StoreRepository(context));
        _productRepository = new Lazy<IProductRepository>(() => new ProductRepository(context));
        _productImageRepository = new Lazy<IProductImageRepository>(() => new ProductImageRepository(context));
        _inventoryRepository = new Lazy<IInventoryRepository>(() => new InventoryRepository(context));
    }
    public IBrandRepository BrandRepository => _brandRepository.Value;

    public IStoreRepository StoreRepository => _storeRepository.Value;

    public IProductRepository ProductRepository => _productRepository.Value;

    public IProductImageRepository ProductImageRepository => _productImageRepository.Value;

    public IInventoryRepository InventoryRepository => _inventoryRepository.Value;

    public async Task<int> SaveAsync()
        => await _context.SaveChangesAsync();
}