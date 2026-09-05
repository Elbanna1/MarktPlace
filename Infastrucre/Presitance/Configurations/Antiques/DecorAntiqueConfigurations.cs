using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class DecorAntiqueConfiguration : IEntityTypeConfiguration<DecorAntique>
{
    public void Configure(EntityTypeBuilder<DecorAntique> builder)
    {
        builder.ToTable("DecorAntiques");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SellerName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(20);
        builder.Property(x => x.WhatsApp).HasMaxLength(20);
        builder.Property(x => x.ItemName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.OtherItemType).HasMaxLength(150);
        builder.Property(x => x.OtherMaterial).HasMaxLength(150);
        builder.Property(x => x.Governorate).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Center).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(300);
        builder.Property(x => x.GoogleMaps).HasMaxLength(1000);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(4000);
        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Length).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Width).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Height).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Weight).HasColumnType("decimal(18,2)");

        builder.Property(x => x.ItemType).HasConversion<int>();
        builder.Property(x => x.Material).HasConversion<int>();
        builder.Property(x => x.Condition).HasConversion<int>();
        builder.Property(x => x.Originality).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.DecorAntique)
            .HasForeignKey(i => i.DecorAntiqueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Video)
            .WithOne(v => v.DecorAntique)
            .HasForeignKey<DecorAntiqueVideo>(v => v.DecorAntiqueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.SellerName);
        builder.HasIndex(x => x.ItemType);
        builder.HasIndex(x => x.Material);
        builder.HasIndex(x => x.Condition);
        builder.HasIndex(x => x.Originality);
        builder.HasIndex(x => x.Negotiable);
        builder.HasIndex(x => x.Center);
        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.UserId);
    }
}

public class DecorAntiqueImageConfiguration : IEntityTypeConfiguration<DecorAntiqueImage>
{
    public void Configure(EntityTypeBuilder<DecorAntiqueImage> builder)
    {
        builder.ToTable("DecorAntiqueImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.DecorAntiqueId);

        builder.HasQueryFilter(i => !i.DecorAntique.IsDeleted);
    }
}

public class DecorAntiqueVideoConfiguration : IEntityTypeConfiguration<DecorAntiqueVideo>
{
    public void Configure(EntityTypeBuilder<DecorAntiqueVideo> builder)
    {
        builder.ToTable("DecorAntiqueVideos");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.FileName).IsRequired().HasMaxLength(256);
        builder.Property(v => v.VideoPath).IsRequired().HasMaxLength(512);
        builder.Property(v => v.VideoUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(v => v.DecorAntiqueId).IsUnique();

        builder.HasQueryFilter(v => !v.DecorAntique.IsDeleted);
    }
}

public class DecorAntiqueItemTypeLookupConfiguration : IEntityTypeConfiguration<DecorAntiqueItemTypeLookup>
{
    public void Configure(EntityTypeBuilder<DecorAntiqueItemTypeLookup> builder)
    {
        builder.ToTable("DecorAntiqueItemTypes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(DecorAntiqueCatalog.ItemTypes
            .Select(entry => new DecorAntiqueItemTypeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class DecorAntiqueMaterialLookupConfiguration : IEntityTypeConfiguration<DecorAntiqueMaterialLookup>
{
    public void Configure(EntityTypeBuilder<DecorAntiqueMaterialLookup> builder)
    {
        builder.ToTable("DecorAntiqueMaterials");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(DecorAntiqueCatalog.Materials
            .Select(entry => new DecorAntiqueMaterialLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class DecorAntiqueConditionLookupConfiguration : IEntityTypeConfiguration<DecorAntiqueConditionLookup>
{
    public void Configure(EntityTypeBuilder<DecorAntiqueConditionLookup> builder)
    {
        builder.ToTable("DecorAntiqueConditions");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(DecorAntiqueCatalog.Conditions
            .Select(entry => new DecorAntiqueConditionLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class DecorAntiqueOriginalityLookupConfiguration : IEntityTypeConfiguration<DecorAntiqueOriginalityLookup>
{
    public void Configure(EntityTypeBuilder<DecorAntiqueOriginalityLookup> builder)
    {
        builder.ToTable("DecorAntiqueOriginalities");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(DecorAntiqueCatalog.Originalities
            .Select(entry => new DecorAntiqueOriginalityLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
