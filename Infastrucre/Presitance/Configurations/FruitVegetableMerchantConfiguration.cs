using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class FruitVegetableMerchantConfiguration : IEntityTypeConfiguration<FruitVegetableMerchant>
{
    public void Configure(EntityTypeBuilder<FruitVegetableMerchant> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.StallName).IsRequired().HasMaxLength(150);
        builder.Property(m => m.MerchantName).IsRequired().HasMaxLength(150);
        builder.Property(m => m.Phone).IsRequired().HasMaxLength(20);

        builder.Property(m => m.WhatsApp).HasMaxLength(20);
        builder.Property(m => m.Address).IsRequired().HasMaxLength(300);
        builder.Property(m => m.GoogleMaps).HasMaxLength(1000);
        builder.Property(m => m.ProductName).IsRequired().HasMaxLength(300);
        builder.Property(m => m.ProductDetails).IsRequired().HasMaxLength(4000);
        builder.Property(m => m.Title).IsRequired().HasMaxLength(150);
        builder.Property(m => m.Description).IsRequired().HasMaxLength(4000);
        builder.Property(m => m.UserId).IsRequired();

        builder.Property(m => m.SaleType).HasConversion<int>();

        builder.HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(m => m.Images)
            .WithOne(i => i.FruitVegetableMerchant)
            .HasForeignKey(i => i.FruitVegetableMerchantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, m => !m.IsDeleted);

        builder.HasIndex(m => m.CreatedAt);
        builder.HasIndex(m => m.MerchantName);
        builder.HasIndex(m => m.ProductName);
        builder.HasIndex(m => m.SaleType);
        builder.HasIndex(m => m.UserId);
    }
}
