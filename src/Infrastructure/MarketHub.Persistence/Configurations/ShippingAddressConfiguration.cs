using MarketHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketHub.Persistence.Configurations;

public class ShippingAddressConfiguration : IEntityTypeConfiguration<ShippingAddress>
{
    public void Configure(EntityTypeBuilder<ShippingAddress> builder)
    {
        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Country)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Governorate)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Street)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.BuildingNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Floor)
            .HasMaxLength(20);

        builder.Property(x => x.Apartment)
            .HasMaxLength(20);

        builder.Property(x => x.PostalCode)
            .HasMaxLength(20);

        builder.Property(x => x.IsDefault)
            .HasDefaultValue(false);

        builder.HasOne(x => x.User)
            .WithMany(x => x.ShippingAddresses)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Orders)
            .WithOne(x => x.ShippingAddressEntity)
            .HasForeignKey(x => x.ShippingAddressEntityId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}