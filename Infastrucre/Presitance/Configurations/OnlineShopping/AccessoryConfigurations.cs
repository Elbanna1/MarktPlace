using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class AccessoryConfiguration : IEntityTypeConfiguration<Accessory>
{
    public void Configure(EntityTypeBuilder<Accessory> builder)
    {
        builder.ToTable("Accessories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StoreName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(20);
        builder.Property(x => x.WhatsApp).IsRequired().HasMaxLength(20);
        builder.Property(x => x.LogoPath).HasMaxLength(500);
        builder.Property(x => x.LogoUrl).HasMaxLength(1000);
        builder.Property(x => x.OtherAccessoryType).HasMaxLength(150);
        builder.Property(x => x.OtherMaterial).HasMaxLength(150);
        builder.Property(x => x.OtherColor).HasMaxLength(150);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(4000);
        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

        builder.Property(x => x.VideoPath).HasMaxLength(500);
        builder.Property(x => x.VideoUrl).HasMaxLength(1000);

        builder.Property(x => x.AccessoryType).HasConversion<int>();
        builder.Property(x => x.Category).HasConversion<int>();
        builder.Property(x => x.Material).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.Accessory)
            .HasForeignKey(i => i.AccessoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Colors)
            .WithOne(c => c.Accessory)
            .HasForeignKey(c => c.AccessoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.StoreName);
        builder.HasIndex(x => x.AccessoryType);
        builder.HasIndex(x => x.Category);
        builder.HasIndex(x => x.Material);
        builder.HasIndex(x => x.ShippingAvailable);
        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.UserId);
    }
}

public class AccessoryImageConfiguration : IEntityTypeConfiguration<AccessoryImage>
{
    public void Configure(EntityTypeBuilder<AccessoryImage> builder)
    {
        builder.ToTable("AccessoryImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.AccessoryId);

        builder.HasQueryFilter(i => !i.Accessory.IsDeleted);
    }
}

public class AccessoryColorSelectionConfiguration : IEntityTypeConfiguration<AccessoryColorSelection>
{
    public void Configure(EntityTypeBuilder<AccessoryColorSelection> builder)
    {
        builder.ToTable("AccessoryColorSelections");

        builder.HasKey(x => new { x.AccessoryId, x.Color });
        builder.Property(x => x.Color).HasConversion<int>();

        builder.HasIndex(x => x.Color);

        builder.HasQueryFilter(x => !x.Accessory.IsDeleted);
    }
}

public class AccessoryTypeLookupConfiguration : IEntityTypeConfiguration<AccessoryTypeLookup>
{
    public void Configure(EntityTypeBuilder<AccessoryTypeLookup> builder)
    {
        builder.ToTable("AccessoryTypes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(AccessoryCatalog.AccessoryTypes
            .Select(entry => new AccessoryTypeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class AccessoryCategoryLookupConfiguration : IEntityTypeConfiguration<AccessoryCategoryLookup>
{
    public void Configure(EntityTypeBuilder<AccessoryCategoryLookup> builder)
    {
        builder.ToTable("AccessoryCategories");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(AccessoryCatalog.Categories
            .Select(entry => new AccessoryCategoryLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class AccessoryMaterialLookupConfiguration : IEntityTypeConfiguration<AccessoryMaterialLookup>
{
    public void Configure(EntityTypeBuilder<AccessoryMaterialLookup> builder)
    {
        builder.ToTable("AccessoryMaterials");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(AccessoryCatalog.Materials
            .Select(entry => new AccessoryMaterialLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class AccessoryColorLookupConfiguration : IEntityTypeConfiguration<AccessoryColorLookup>
{
    public void Configure(EntityTypeBuilder<AccessoryColorLookup> builder)
    {
        builder.ToTable("AccessoryColors");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(AccessoryCatalog.Colors
            .Select(entry => new AccessoryColorLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
