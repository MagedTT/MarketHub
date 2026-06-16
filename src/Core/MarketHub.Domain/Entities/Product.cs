using System.ComponentModel.DataAnnotations;

namespace MarketHub.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }

    public Guid StoreId { get; set; }
    public Store Store { get; set; } = default!;

    public Guid? BrandId { get; set; }
    public Brand? Brand { get; set; } = default!;

    public Inventory Inventory { get; set; } = default!;

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string Type { get; set; } = string.Empty; // phone, tv, cloths, books, perfume, other.
    public string Specifications { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
    public DateTime DeletedAt { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = default!;

    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<Report> Reports { get; set; } = new List<Report>();
    public ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}