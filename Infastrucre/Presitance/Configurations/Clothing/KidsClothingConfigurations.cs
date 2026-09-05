using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class KidsClothingConfiguration : IEntityTypeConfiguration<KidsClothing>
{
    public void Configure(EntityTypeBuilder<KidsClothing> builder)
    {
        builder.ToTable("KidsClothings");

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
            .WithOne(i => i.KidsClothing)
            .HasForeignKey(i => i.KidsClothingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Sizes)
            .WithOne(s => s.KidsClothing)
            .HasForeignKey(s => s.KidsClothingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Colors)
            .WithOne(c => c.KidsClothing)
            .HasForeignKey(c => c.KidsClothingId)
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

public class KidsClothingImageConfiguration : IEntityTypeConfiguration<KidsClothingImage>
{
    public void Configure(EntityTypeBuilder<KidsClothingImage> builder)
    {
        builder.ToTable("KidsClothingImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.KidsClothingId);

        builder.HasQueryFilter(i => !i.KidsClothing.IsDeleted);
    }
}

public class KidsClothingSizeSelectionConfiguration : IEntityTypeConfiguration<KidsClothingSizeSelection>
{
    public void Configure(EntityTypeBuilder<KidsClothingSizeSelection> builder)
    {
        builder.ToTable("KidsClothingSizeSelections");

        builder.HasKey(x => new { x.KidsClothingId, x.Size });
        builder.Property(x => x.Size).HasConversion<int>();

        builder.HasIndex(x => x.Size);

        builder.HasQueryFilter(x => !x.KidsClothing.IsDeleted);
    }
}

public class KidsClothingColorSelectionConfiguration : IEntityTypeConfiguration<KidsClothingColorSelection>
{
    public void Configure(EntityTypeBuilder<KidsClothingColorSelection> builder)
    {
        builder.ToTable("KidsClothingColorSelections");

        builder.HasKey(x => new { x.KidsClothingId, x.Color });
        builder.Property(x => x.Color).HasConversion<int>();

        builder.HasIndex(x => x.Color);

        builder.HasQueryFilter(x => !x.KidsClothing.IsDeleted);
    }
}

public class KidsClothingTypeLookupConfiguration : IEntityTypeConfiguration<KidsClothingTypeLookup>
{
    public void Configure(EntityTypeBuilder<KidsClothingTypeLookup> builder)
    {
        builder.ToTable("KidsClothingTypes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(KidsClothingCatalog.ClothingTypes
            .Select(entry => new KidsClothingTypeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class KidsClothingBrandLookupConfiguration : IEntityTypeConfiguration<KidsClothingBrandLookup>
{
    public void Configure(EntityTypeBuilder<KidsClothingBrandLookup> builder)
    {
        builder.ToTable("KidsClothingBrands");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(KidsClothingCatalog.Brands
            .Select(entry => new KidsClothingBrandLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class KidsClothingSizeLookupConfiguration : IEntityTypeConfiguration<KidsClothingSizeLookup>
{
    public void Configure(EntityTypeBuilder<KidsClothingSizeLookup> builder)
    {
        builder.ToTable("KidsClothingSizes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(KidsClothingCatalog.Sizes
            .Select(entry => new KidsClothingSizeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class KidsClothingColorLookupConfiguration : IEntityTypeConfiguration<KidsClothingColorLookup>
{
    public void Configure(EntityTypeBuilder<KidsClothingColorLookup> builder)
    {
        builder.ToTable("KidsClothingColors");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(KidsClothingCatalog.Colors
            .Select(entry => new KidsClothingColorLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class KidsClothingConditionLookupConfiguration : IEntityTypeConfiguration<KidsClothingConditionLookup>
{
    public void Configure(EntityTypeBuilder<KidsClothingConditionLookup> builder)
    {
        builder.ToTable("KidsClothingConditions");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(KidsClothingCatalog.Conditions
            .Select(entry => new KidsClothingConditionLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class KidsClothingSellingMethodLookupConfiguration
    : IEntityTypeConfiguration<KidsClothingSellingMethodLookup>
{
    public void Configure(EntityTypeBuilder<KidsClothingSellingMethodLookup> builder)
    {
        builder.ToTable("KidsClothingSellingMethods");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(KidsClothingCatalog.SellingMethods
            .Select(entry => new KidsClothingSellingMethodLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
