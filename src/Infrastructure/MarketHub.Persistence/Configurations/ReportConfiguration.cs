using MarketHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketHub.Persistence.Configurations;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasOne(x => x.ReporterUser)
            .WithMany(x => x.Reports)
            .HasForeignKey(x => x.ReporterUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.Reports)
            .HasForeignKey(x => x.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Store)
            .WithMany(x => x.Reports)
            .HasForeignKey(x => x.StoreId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}