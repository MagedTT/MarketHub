using System.ComponentModel.DataAnnotations;

namespace MarketHub.Domain.Entities;

public class Inventory
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;

    public int Quantity { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = default!;
}