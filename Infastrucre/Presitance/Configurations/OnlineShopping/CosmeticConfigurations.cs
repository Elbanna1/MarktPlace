using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class CosmeticConfiguration : IEntityTypeConfiguration<Cosmetic>
{
    public void Configure(EntityTypeBuilder<Cosmetic> builder)
    {
        builder.ToTable("Cosmetics");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StoreName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(20);
        builder.Property(x => x.WhatsApp).IsRequired().HasMaxLength(20);
        builder.Property(x => x.OtherSection).HasMaxLength(150);
        builder.Property(x => x.Brand).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(4000);
        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

        builder.Property(x => x.VideoPath).HasMaxLength(500);
        builder.Property(x => x.VideoUrl).HasMaxLength(1000);

        builder.Property(x => x.Section).HasConversion<int>();
        builder.Property(x => x.SuitableFor).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.Cosmetic)
            .HasForeignKey(i => i.CosmeticId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.StoreName);
        builder.HasIndex(x => x.Section);
        builder.HasIndex(x => x.Brand);
        builder.HasIndex(x => x.SuitableFor);
        builder.HasIndex(x => x.DiscountAvailable);
        builder.HasIndex(x => x.ShippingAvailable);
        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.UserId);
    }
}

public class CosmeticImageConfiguration : IEntityTypeConfiguration<CosmeticImage>
{
    public void Configure(EntityTypeBuilder<CosmeticImage> builder)
    {
        builder.ToTable("CosmeticImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.CosmeticId);

        builder.HasQueryFilter(i => !i.Cosmetic.IsDeleted);
    }
}

public class CosmeticSectionLookupConfiguration : IEntityTypeConfiguration<CosmeticSectionLookup>
{
    public void Configure(EntityTypeBuilder<CosmeticSectionLookup> builder)
    {
        builder.ToTable("CosmeticSections");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(CosmeticCatalog.Sections
            .Select(entry => new CosmeticSectionLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class CosmeticSuitableForLookupConfiguration : IEntityTypeConfiguration<CosmeticSuitableForLookup>
{
    public void Configure(EntityTypeBuilder<CosmeticSuitableForLookup> builder)
    {
        builder.ToTable("CosmeticSuitableFor");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(CosmeticCatalog.SuitableFor
            .Select(entry => new CosmeticSuitableForLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
