using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class WholesaleTraderConfiguration : IEntityTypeConfiguration<WholesaleTrader>
{
    public void Configure(EntityTypeBuilder<WholesaleTrader> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.TraderName).IsRequired().HasMaxLength(150);
        builder.Property(t => t.OtherTradeType).HasMaxLength(150);
        builder.Property(t => t.ProductsName).IsRequired().HasMaxLength(300);
        builder.Property(t => t.ProductDetails).IsRequired().HasMaxLength(4000);
        builder.Property(t => t.Address).IsRequired().HasMaxLength(300);
        builder.Property(t => t.GoogleMaps).HasMaxLength(1000);
        builder.Property(t => t.Phone).IsRequired().HasMaxLength(20);
        builder.Property(t => t.WhatsApp).IsRequired().HasMaxLength(20);
        builder.Property(t => t.Email).HasMaxLength(256);
        builder.Property(t => t.Title).IsRequired().HasMaxLength(150);
        builder.Property(t => t.Description).IsRequired().HasMaxLength(4000);
        builder.Property(t => t.UserId).IsRequired();

        builder.Property(t => t.TradeType).HasConversion<int>();
        builder.Property(t => t.SaleType).HasConversion<int>();

        builder.HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(t => t.Images)
            .WithOne(i => i.WholesaleTrader)
            .HasForeignKey(i => i.WholesaleTraderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, t => !t.IsDeleted);

        builder.HasIndex(t => t.CreatedAt);
        builder.HasIndex(t => t.TraderName);
        builder.HasIndex(t => t.TradeType);
        builder.HasIndex(t => t.SaleType);
        builder.HasIndex(t => t.UserId);
    }
}
