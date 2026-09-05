using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class MenClothingConfiguration : IEntityTypeConfiguration<MenClothing>
{
    public void Configure(EntityTypeBuilder<MenClothing> builder)
    {
        builder.ToTable("MenClothings");

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
            .WithOne(i => i.MenClothing)
            .HasForeignKey(i => i.MenClothingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Sizes)
            .WithOne(s => s.MenClothing)
            .HasForeignKey(s => s.MenClothingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Colors)
            .WithOne(c => c.MenClothing)
            .HasForeignKey(c => c.MenClothingId)
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

public class MenClothingImageConfiguration : IEntityTypeConfiguration<MenClothingImage>
{
    public void Configure(EntityTypeBuilder<MenClothingImage> builder)
    {
        builder.ToTable("MenClothingImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.MenClothingId);

        builder.HasQueryFilter(i => !i.MenClothing.IsDeleted);
    }
}

public class MenClothingSizeSelectionConfiguration : IEntityTypeConfiguration<MenClothingSizeSelection>
{
    public void Configure(EntityTypeBuilder<MenClothingSizeSelection> builder)
    {
        builder.ToTable("MenClothingSizeSelections");

        builder.HasKey(x => new { x.MenClothingId, x.Size });
        builder.Property(x => x.Size).HasConversion<int>();

        builder.HasIndex(x => x.Size);

        builder.HasQueryFilter(x => !x.MenClothing.IsDeleted);
    }
}

public class MenClothingColorSelectionConfiguration : IEntityTypeConfiguration<MenClothingColorSelection>
{
    public void Configure(EntityTypeBuilder<MenClothingColorSelection> builder)
    {
        builder.ToTable("MenClothingColorSelections");

        builder.HasKey(x => new { x.MenClothingId, x.Color });
        builder.Property(x => x.Color).HasConversion<int>();

        builder.HasIndex(x => x.Color);

        builder.HasQueryFilter(x => !x.MenClothing.IsDeleted);
    }
}

public class MenClothingTypeLookupConfiguration : IEntityTypeConfiguration<MenClothingTypeLookup>
{
    public void Configure(EntityTypeBuilder<MenClothingTypeLookup> builder)
    {
        builder.ToTable("MenClothingTypes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(MenClothingCatalog.ClothingTypes
            .Select(entry => new MenClothingTypeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class MenClothingBrandLookupConfiguration : IEntityTypeConfiguration<MenClothingBrandLookup>
{
    public void Configure(EntityTypeBuilder<MenClothingBrandLookup> builder)
    {
        builder.ToTable("MenClothingBrands");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(MenClothingCatalog.Brands
            .Select(entry => new MenClothingBrandLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class MenClothingSizeLookupConfiguration : IEntityTypeConfiguration<MenClothingSizeLookup>
{
    public void Configure(EntityTypeBuilder<MenClothingSizeLookup> builder)
    {
        builder.ToTable("MenClothingSizes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(MenClothingCatalog.Sizes
            .Select(entry => new MenClothingSizeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class MenClothingColorLookupConfiguration : IEntityTypeConfiguration<MenClothingColorLookup>
{
    public void Configure(EntityTypeBuilder<MenClothingColorLookup> builder)
    {
        builder.ToTable("MenClothingColors");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(MenClothingCatalog.Colors
            .Select(entry => new MenClothingColorLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class MenClothingConditionLookupConfiguration : IEntityTypeConfiguration<MenClothingConditionLookup>
{
    public void Configure(EntityTypeBuilder<MenClothingConditionLookup> builder)
    {
        builder.ToTable("MenClothingConditions");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(MenClothingCatalog.Conditions
            .Select(entry => new MenClothingConditionLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class MenClothingSellingMethodLookupConfiguration
    : IEntityTypeConfiguration<MenClothingSellingMethodLookup>
{
    public void Configure(EntityTypeBuilder<MenClothingSellingMethodLookup> builder)
    {
        builder.ToTable("MenClothingSellingMethods");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(MenClothingCatalog.SellingMethods
            .Select(entry => new MenClothingSellingMethodLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
