using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;

namespace MarketHub.Persistence.Repositories;

public class RepositoryManager : IRepositoryManager
{
    protected readonly MarketHubDbContext _context;
    private readonly Lazy<IBrandRepository> _brandRepository;
    private readonly Lazy<IStoreRepository> _storeRepository;
    private readonly Lazy<IProductRepository> _productRepository;
    private readonly Lazy<IProductImageRepository> _productImageRepository;
    private readonly Lazy<IInventoryRepository> _inventoryRepository;
    private readonly Lazy<ICartRepository> _cartRepository;
    private readonly Lazy<ICartItemRepository> _cartItemRepository;
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<IWishlistReposiotry> _wishlistRepository;
    private readonly Lazy<IReviewRepository> _reviewRepository;
    private readonly Lazy<IInventoryReservationRepository> _inventoryReservationRepository;
    private readonly Lazy<IOrdersRepository> _ordersRepository;
    private readonly Lazy<IPromoCodeRepository> _promoCodeRepository;

    public RepositoryManager(MarketHubDbContext context)
    {
        _context = context;
        _brandRepository = new Lazy<IBrandRepository>(() => new BrandRepository(context));
        _storeRepository = new Lazy<IStoreRepository>(() => new StoreRepository(context));
        _productRepository = new Lazy<IProductRepository>(() => new ProductRepository(context));
        _productImageRepository = new Lazy<IProductImageRepository>(() => new ProductImageRepository(context));
        _inventoryRepository = new Lazy<IInventoryRepository>(() => new InventoryRepository(context));
        _cartRepository = new Lazy<ICartRepository>(() => new CartRepository(context));
        _cartItemRepository = new Lazy<ICartItemRepository>(() => new CartItemRepository(context));
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(context));
        _wishlistRepository = new Lazy<IWishlistReposiotry>(() => new WishlistRepository(context));
        _reviewRepository = new Lazy<IReviewRepository>(() => new ReviewRepository(context));
        _ordersRepository = new Lazy<IOrdersRepository>(() => new OrdersRepository(context));
        _inventoryReservationRepository = new Lazy<IInventoryReservationRepository>(() => new InventoryReservationRepository(context));
        _promoCodeRepository = new Lazy<IPromoCodeRepository>(() => new PromoCodeRepository(context));
    }
    public IBrandRepository BrandRepository => _brandRepository.Value;

    public IStoreRepository StoreRepository => _storeRepository.Value;

    public IProductRepository ProductRepository => _productRepository.Value;

    public IProductImageRepository ProductImageRepository => _productImageRepository.Value;

    public IInventoryRepository InventoryRepository => _inventoryRepository.Value;

    public ICartRepository CartRepository => _cartRepository.Value;

    public ICartItemRepository CartItemRepository => _cartItemRepository.Value;

    public IUserRepository UserRepository => _userRepository.Value;

    public IWishlistReposiotry WishlistRepository => _wishlistRepository.Value;

    public IReviewRepository ReviewRepository => _reviewRepository.Value;

    public IInventoryReservationRepository InventoryReservationRepository => _inventoryReservationRepository.Value;

    public IOrdersRepository OrdersRepository => _ordersRepository.Value;

    public IPromoCodeRepository PromoCodeRepository => _promoCodeRepository.Value;

    public async Task<int> SaveAsync()
        => await _context.SaveChangesAsync();
}