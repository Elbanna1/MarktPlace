using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class HomeKitchenConfiguration : IEntityTypeConfiguration<HomeKitchen>
{
    public void Configure(EntityTypeBuilder<HomeKitchen> builder)
    {
        builder.ToTable("HomeKitchens");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StoreName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(20);
        builder.Property(x => x.WhatsApp).IsRequired().HasMaxLength(20);
        builder.Property(x => x.OtherSection).HasMaxLength(150);
        builder.Property(x => x.OtherMaterial).HasMaxLength(150);
        builder.Property(x => x.OtherColor).HasMaxLength(150);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(4000);
        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

        builder.Property(x => x.VideoPath).HasMaxLength(500);
        builder.Property(x => x.VideoUrl).HasMaxLength(1000);

        builder.Property(x => x.Section).HasConversion<int>();
        builder.Property(x => x.Material).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.HomeKitchen)
            .HasForeignKey(i => i.HomeKitchenId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Colors)
            .WithOne(c => c.HomeKitchen)
            .HasForeignKey(c => c.HomeKitchenId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.StoreName);
        builder.HasIndex(x => x.Section);
        builder.HasIndex(x => x.Material);
        builder.HasIndex(x => x.DeliveryAvailable);
        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.UserId);
    }
}

public class HomeKitchenImageConfiguration : IEntityTypeConfiguration<HomeKitchenImage>
{
    public void Configure(EntityTypeBuilder<HomeKitchenImage> builder)
    {
        builder.ToTable("HomeKitchenImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.HomeKitchenId);

        builder.HasQueryFilter(i => !i.HomeKitchen.IsDeleted);
    }
}

public class HomeKitchenColorSelectionConfiguration : IEntityTypeConfiguration<HomeKitchenColorSelection>
{
    public void Configure(EntityTypeBuilder<HomeKitchenColorSelection> builder)
    {
        builder.ToTable("HomeKitchenColorSelections");

        builder.HasKey(x => new { x.HomeKitchenId, x.Color });
        builder.Property(x => x.Color).HasConversion<int>();

        builder.HasIndex(x => x.Color);

        builder.HasQueryFilter(x => !x.HomeKitchen.IsDeleted);
    }
}

public class HomeKitchenSectionLookupConfiguration : IEntityTypeConfiguration<HomeKitchenSectionLookup>
{
    public void Configure(EntityTypeBuilder<HomeKitchenSectionLookup> builder)
    {
        builder.ToTable("HomeKitchenSections");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(HomeKitchenCatalog.Sections
            .Select(entry => new HomeKitchenSectionLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class HomeKitchenMaterialLookupConfiguration : IEntityTypeConfiguration<HomeKitchenMaterialLookup>
{
    public void Configure(EntityTypeBuilder<HomeKitchenMaterialLookup> builder)
    {
        builder.ToTable("HomeKitchenMaterials");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(HomeKitchenCatalog.Materials
            .Select(entry => new HomeKitchenMaterialLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class HomeKitchenColorLookupConfiguration : IEntityTypeConfiguration<HomeKitchenColorLookup>
{
    public void Configure(EntityTypeBuilder<HomeKitchenColorLookup> builder)
    {
        builder.ToTable("HomeKitchenColors");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(HomeKitchenCatalog.Colors
            .Select(entry => new HomeKitchenColorLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
