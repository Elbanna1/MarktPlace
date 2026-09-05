using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class WomenClothingConfiguration : IEntityTypeConfiguration<WomenClothing>
{
    public void Configure(EntityTypeBuilder<WomenClothing> builder)
    {
        builder.ToTable("WomenClothings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StoreName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.OtherClothingType).HasMaxLength(150);
        builder.Property(x => x.OtherBrand).HasMaxLength(150);
        builder.Property(x => x.OtherColor).HasMaxLength(150);
        builder.Property(x => x.Governorate).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Center).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(300);
        builder.Property(x => x.GoogleMaps).HasMaxLength(1000);
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(20);
        builder.Property(x => x.WhatsApp).IsRequired().HasMaxLength(20);
        builder.Property(x => x.Email).HasMaxLength(256);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(4000);
        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

        builder.Property(x => x.VideoPath).HasMaxLength(500);
        builder.Property(x => x.VideoUrl).HasMaxLength(1000);

        builder.Property(x => x.SellingMethod).HasConversion<int>();
        builder.Property(x => x.ClothingType).HasConversion<int>();
        builder.Property(x => x.Brand).HasConversion<int>();
        builder.Property(x => x.Condition).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.WomenClothing)
            .HasForeignKey(i => i.WomenClothingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Sizes)
            .WithOne(s => s.WomenClothing)
            .HasForeignKey(s => s.WomenClothingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Colors)
            .WithOne(c => c.WomenClothing)
            .HasForeignKey(c => c.WomenClothingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.StoreName);
        builder.HasIndex(x => x.ClothingType);
        builder.HasIndex(x => x.Brand);
        builder.HasIndex(x => x.Condition);
        builder.HasIndex(x => x.SellingMethod);
        builder.HasIndex(x => x.Center);
        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.UserId);
    }
}

public class WomenClothingImageConfiguration : IEntityTypeConfiguration<WomenClothingImage>
{
    public void Configure(EntityTypeBuilder<WomenClothingImage> builder)
    {
        builder.ToTable("WomenClothingImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.WomenClothingId);

        builder.HasQueryFilter(i => !i.WomenClothing.IsDeleted);
    }
}

public class WomenClothingSizeSelectionConfiguration : IEntityTypeConfiguration<WomenClothingSizeSelection>
{
    public void Configure(EntityTypeBuilder<WomenClothingSizeSelection> builder)
    {
        builder.ToTable("WomenClothingSizeSelections");

        builder.HasKey(x => new { x.WomenClothingId, x.Size });
        builder.Property(x => x.Size).HasConversion<int>();

        builder.HasIndex(x => x.Size);

        builder.HasQueryFilter(x => !x.WomenClothing.IsDeleted);
    }
}

public class WomenClothingColorSelectionConfiguration : IEntityTypeConfiguration<WomenClothingColorSelection>
{
    public void Configure(EntityTypeBuilder<WomenClothingColorSelection> builder)
    {
        builder.ToTable("WomenClothingColorSelections");

        builder.HasKey(x => new { x.WomenClothingId, x.Color });
        builder.Property(x => x.Color).HasConversion<int>();

        builder.HasIndex(x => x.Color);

        builder.HasQueryFilter(x => !x.WomenClothing.IsDeleted);
    }
}

public class WomenClothingTypeLookupConfiguration : IEntityTypeConfiguration<WomenClothingTypeLookup>
{
    public void Configure(EntityTypeBuilder<WomenClothingTypeLookup> builder)
    {
        builder.ToTable("WomenClothingTypes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(WomenClothingCatalog.ClothingTypes
            .Select(entry => new WomenClothingTypeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class WomenClothingBrandLookupConfiguration : IEntityTypeConfiguration<WomenClothingBrandLookup>
{
    public void Configure(EntityTypeBuilder<WomenClothingBrandLookup> builder)
    {
        builder.ToTable("WomenClothingBrands");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(WomenClothingCatalog.Brands
            .Select(entry => new WomenClothingBrandLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class WomenClothingSizeLookupConfiguration : IEntityTypeConfiguration<WomenClothingSizeLookup>
{
    public void Configure(EntityTypeBuilder<WomenClothingSizeLookup> builder)
    {
        builder.ToTable("WomenClothingSizes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(WomenClothingCatalog.Sizes
            .Select(entry => new WomenClothingSizeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class WomenClothingColorLookupConfiguration : IEntityTypeConfiguration<WomenClothingColorLookup>
{
    public void Configure(EntityTypeBuilder<WomenClothingColorLookup> builder)
    {
        builder.ToTable("WomenClothingColors");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(WomenClothingCatalog.Colors
            .Select(entry => new WomenClothingColorLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class WomenClothingConditionLookupConfiguration : IEntityTypeConfiguration<WomenClothingConditionLookup>
{
    public void Configure(EntityTypeBuilder<WomenClothingConditionLookup> builder)
    {
        builder.ToTable("WomenClothingConditions");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(WomenClothingCatalog.Conditions
            .Select(entry => new WomenClothingConditionLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class WomenClothingSellingMethodLookupConfiguration
    : IEntityTypeConfiguration<WomenClothingSellingMethodLookup>
{
    public void Configure(EntityTypeBuilder<WomenClothingSellingMethodLookup> builder)
    {
        builder.ToTable("WomenClothingSellingMethods");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(WomenClothingCatalog.SellingMethods
            .Select(entry => new WomenClothingSellingMethodLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
