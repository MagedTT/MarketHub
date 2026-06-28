namespace MarketHub.Application.DTOs.Persistence.Inventories;

public class InventoryDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
    public bool ProductIsActive { get; set; }
    public int AvailableQuantity { get; set; }
    public int ReservedQuantity { get; set; }
}