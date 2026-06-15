namespace MarketHub.Domain.Entities;

public class Store
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Report> Reports { get; set; } = new List<Report>();
}