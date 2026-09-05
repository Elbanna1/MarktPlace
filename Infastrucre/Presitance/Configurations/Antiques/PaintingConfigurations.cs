using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class PaintingConfiguration : IEntityTypeConfiguration<Painting>
{
    public void Configure(EntityTypeBuilder<Painting> builder)
    {
        builder.ToTable("Paintings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SellerName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(20);
        builder.Property(x => x.WhatsApp).HasMaxLength(20);
        builder.Property(x => x.PaintingName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.OtherType).HasMaxLength(150);
        builder.Property(x => x.ArtistName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.OtherMaterial).HasMaxLength(150);
        builder.Property(x => x.Governorate).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Center).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(300);
        builder.Property(x => x.GoogleMaps).HasMaxLength(1000);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(4000);
        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Width).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Height).HasColumnType("decimal(18,2)");

        builder.Property(x => x.PaintingType).HasConversion<int>();
        builder.Property(x => x.Material).HasConversion<int>();
        builder.Property(x => x.Originality).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.Painting)
            .HasForeignKey(i => i.PaintingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Video)
            .WithOne(v => v.Painting)
            .HasForeignKey<PaintingVideo>(v => v.PaintingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.SellerName);
        builder.HasIndex(x => x.PaintingType);
        builder.HasIndex(x => x.ArtistName);
        builder.HasIndex(x => x.Originality);
        builder.HasIndex(x => x.Framed);
        builder.HasIndex(x => x.Negotiable);
        builder.HasIndex(x => x.Center);
        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.UserId);
    }
}

public class PaintingImageConfiguration : IEntityTypeConfiguration<PaintingImage>
{
    public void Configure(EntityTypeBuilder<PaintingImage> builder)
    {
        builder.ToTable("PaintingImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.PaintingId);

        builder.HasQueryFilter(i => !i.Painting.IsDeleted);
    }
}

public class PaintingVideoConfiguration : IEntityTypeConfiguration<PaintingVideo>
{
    public void Configure(EntityTypeBuilder<PaintingVideo> builder)
    {
        builder.ToTable("PaintingVideos");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.FileName).IsRequired().HasMaxLength(256);
        builder.Property(v => v.VideoPath).IsRequired().HasMaxLength(512);
        builder.Property(v => v.VideoUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(v => v.PaintingId).IsUnique();

        builder.HasQueryFilter(v => !v.Painting.IsDeleted);
    }
}

public class PaintingTypeLookupConfiguration : IEntityTypeConfiguration<PaintingTypeLookup>
{
    public void Configure(EntityTypeBuilder<PaintingTypeLookup> builder)
    {
        builder.ToTable("PaintingTypes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(PaintingCatalog.Types
            .Select(entry => new PaintingTypeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class PaintingMaterialLookupConfiguration : IEntityTypeConfiguration<PaintingMaterialLookup>
{
    public void Configure(EntityTypeBuilder<PaintingMaterialLookup> builder)
    {
        builder.ToTable("PaintingMaterials");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(PaintingCatalog.Materials
            .Select(entry => new PaintingMaterialLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class PaintingOriginalityLookupConfiguration : IEntityTypeConfiguration<PaintingOriginalityLookup>
{
    public void Configure(EntityTypeBuilder<PaintingOriginalityLookup> builder)
    {
        builder.ToTable("PaintingOriginalities");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(PaintingCatalog.Originalities
            .Select(entry => new PaintingOriginalityLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
