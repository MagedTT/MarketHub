namespace MarketHub.Domain.Entities;

public class ProductImage
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;

    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } // max of 5 pictures
}