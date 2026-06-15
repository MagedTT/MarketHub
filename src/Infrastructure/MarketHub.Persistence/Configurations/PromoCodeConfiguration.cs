using MarketHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketHub.Persistence.Configurations;

public class PromoCodeConfiguration : IEntityTypeConfiguration<PromoCode>
{
    public void Configure(EntityTypeBuilder<PromoCode> builder)
    {
        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.Property(x => x.DiscountType)
            .IsRequired();

        builder.Property(x => x.DiscountValue)
            .HasPrecision(6, 2);

        builder.HasMany(x => x.Orders)
            .WithOne(x => x.PromoCode)
            .HasForeignKey(x => x.PromoCodeId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}