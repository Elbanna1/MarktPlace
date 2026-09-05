using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class ShoppingElectronicConfiguration : IEntityTypeConfiguration<ShoppingElectronic>
{
    public void Configure(EntityTypeBuilder<ShoppingElectronic> builder)
    {
        builder.ToTable("ShoppingElectronics");

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
        builder.Property(x => x.CompatibleWith).HasConversion<int>();
        builder.Property(x => x.ProductCondition).HasConversion<int>();
        builder.Property(x => x.Warranty).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.ShoppingElectronic)
            .HasForeignKey(i => i.ShoppingElectronicId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.StoreName);
        builder.HasIndex(x => x.Section);
        builder.HasIndex(x => x.Brand);
        builder.HasIndex(x => x.CompatibleWith);
        builder.HasIndex(x => x.ProductCondition);
        builder.HasIndex(x => x.Warranty);
        builder.HasIndex(x => x.ShippingAvailable);
        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.UserId);
    }
}

public class ShoppingElectronicImageConfiguration : IEntityTypeConfiguration<ShoppingElectronicImage>
{
    public void Configure(EntityTypeBuilder<ShoppingElectronicImage> builder)
    {
        builder.ToTable("ShoppingElectronicImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.ShoppingElectronicId);

        builder.HasQueryFilter(i => !i.ShoppingElectronic.IsDeleted);
    }
}

public class ShoppingElectronicSectionLookupConfiguration
    : IEntityTypeConfiguration<ShoppingElectronicSectionLookup>
{
    public void Configure(EntityTypeBuilder<ShoppingElectronicSectionLookup> builder)
    {
        builder.ToTable("ShoppingElectronicSections");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(ShoppingElectronicCatalog.Sections
            .Select(entry => new ShoppingElectronicSectionLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class ShoppingElectronicCompatibilityLookupConfiguration
    : IEntityTypeConfiguration<ShoppingElectronicCompatibilityLookup>
{
    public void Configure(EntityTypeBuilder<ShoppingElectronicCompatibilityLookup> builder)
    {
        builder.ToTable("ShoppingElectronicCompatibilities");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(ShoppingElectronicCatalog.Compatibilities
            .Select(entry => new ShoppingElectronicCompatibilityLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class ShoppingElectronicConditionLookupConfiguration
    : IEntityTypeConfiguration<ShoppingElectronicConditionLookup>
{
    public void Configure(EntityTypeBuilder<ShoppingElectronicConditionLookup> builder)
    {
        builder.ToTable("ShoppingElectronicConditions");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(ShoppingElectronicCatalog.Conditions
            .Select(entry => new ShoppingElectronicConditionLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class ShoppingElectronicWarrantyLookupConfiguration
    : IEntityTypeConfiguration<ShoppingElectronicWarrantyLookup>
{
    public void Configure(EntityTypeBuilder<ShoppingElectronicWarrantyLookup> builder)
    {
        builder.ToTable("ShoppingElectronicWarranties");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(ShoppingElectronicCatalog.Warranties
            .Select(entry => new ShoppingElectronicWarrantyLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
