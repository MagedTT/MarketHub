using MarketHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketHub.Persistence.Configurations;

public class InventoryReservationConfiguration : IEntityTypeConfiguration<InventoryReservation>
{
    public void Configure(EntityTypeBuilder<InventoryReservation> builder)
    {
        builder.HasIndex(r => new { r.UserId, r.ProductId })
            .IsUnique();

        builder.HasOne(x => x.User)
            .WithMany(x => x.InventoryReservations)
            .HasForeignKey(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.InventoryReservations)
            .HasForeignKey(x => x.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Inventory)
            .WithMany(x => x.InventoryReservations)
            .HasForeignKey(x => x.InventoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}