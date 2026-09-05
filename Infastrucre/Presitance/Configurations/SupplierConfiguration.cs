using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.SupplierName).IsRequired().HasMaxLength(150);
        builder.Property(s => s.OtherSupplierType).HasMaxLength(150);
        builder.Property(s => s.SuppliedProduct).IsRequired().HasMaxLength(300);
        builder.Property(s => s.SupplyDetails).IsRequired().HasMaxLength(4000);
        builder.Property(s => s.Address).IsRequired().HasMaxLength(300);
        builder.Property(s => s.GoogleMaps).HasMaxLength(1000);
        builder.Property(s => s.Phone).IsRequired().HasMaxLength(20);
        builder.Property(s => s.WhatsApp).IsRequired().HasMaxLength(20);
        builder.Property(s => s.Email).HasMaxLength(256);
        builder.Property(s => s.Title).IsRequired().HasMaxLength(150);
        builder.Property(s => s.Description).IsRequired().HasMaxLength(4000);
        builder.Property(s => s.UserId).IsRequired();

        builder.Property(s => s.SupplierType).HasConversion<int>();

        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(s => s.Images)
            .WithOne(i => i.Supplier)
            .HasForeignKey(i => i.SupplierId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, s => !s.IsDeleted);

        builder.HasIndex(s => s.CreatedAt);
        builder.HasIndex(s => s.SupplierName);
        builder.HasIndex(s => s.SupplierType);
        builder.HasIndex(s => s.UserId);
    }
}
