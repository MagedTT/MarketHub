using MarketHub.Domain.Entities;
using MarketHub.Persistence.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Persistence;

public class MarketHubDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<InventoryReservation> InventoryReservations { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<PromoCode> PromoCodes { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<ShipmentTracking> ShipmentTrackings { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }
    public DbSet<WishlistItem> WishlistItems { get; set; }

    public MarketHubDbContext(DbContextOptions<MarketHubDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(BrandConfiguration).Assembly);
    }
}
