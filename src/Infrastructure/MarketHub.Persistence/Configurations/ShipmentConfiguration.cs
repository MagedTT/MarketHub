using MarketHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketHub.Persistence.Configurations;

public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.HasIndex(x => x.OrderId)
            .IsUnique();

        builder.HasIndex(x => x.TrackingNumber)
            .IsUnique();

        builder.Property(x => x.Carrier)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(x => x.Order)
            .WithOne(x => x.Shipment)
            .HasForeignKey<Shipment>(x => x.OrderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}