using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class AntiqueConfiguration : IEntityTypeConfiguration<Antique>
{
    public void Configure(EntityTypeBuilder<Antique> builder)
    {
        builder.ToTable("Antiques");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SellerName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(20);
        builder.Property(x => x.WhatsApp).HasMaxLength(20);
        builder.Property(x => x.AntiqueName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.OtherType).HasMaxLength(150);
        builder.Property(x => x.OtherMaterial).HasMaxLength(150);
        builder.Property(x => x.CountryOfOrigin).HasMaxLength(100);
        builder.Property(x => x.Manufacturer).HasMaxLength(150);
        builder.Property(x => x.Governorate).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Center).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(300);
        builder.Property(x => x.GoogleMaps).HasMaxLength(1000);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(4000);
        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

        builder.Property(x => x.AntiqueType).HasConversion<int>();
        builder.Property(x => x.Material).HasConversion<int>();
        builder.Property(x => x.Condition).HasConversion<int>();
        builder.Property(x => x.WorkingStatus).HasConversion<int>();
        builder.Property(x => x.Originality).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.Antique)
            .HasForeignKey(i => i.AntiqueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Video)
            .WithOne(v => v.Antique)
            .HasForeignKey<AntiqueVideo>(v => v.AntiqueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.SellerName);
        builder.HasIndex(x => x.AntiqueType);
        builder.HasIndex(x => x.ManufactureYear);
        builder.HasIndex(x => x.CountryOfOrigin);
        builder.HasIndex(x => x.Condition);
        builder.HasIndex(x => x.Originality);
        builder.HasIndex(x => x.Negotiable);
        builder.HasIndex(x => x.Center);
        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.UserId);
    }
}

public class AntiqueImageConfiguration : IEntityTypeConfiguration<AntiqueImage>
{
    public void Configure(EntityTypeBuilder<AntiqueImage> builder)
    {
        builder.ToTable("AntiqueImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.AntiqueId);

        builder.HasQueryFilter(i => !i.Antique.IsDeleted);
    }
}

public class AntiqueVideoConfiguration : IEntityTypeConfiguration<AntiqueVideo>
{
    public void Configure(EntityTypeBuilder<AntiqueVideo> builder)
    {
        builder.ToTable("AntiqueVideos");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.FileName).IsRequired().HasMaxLength(256);
        builder.Property(v => v.VideoPath).IsRequired().HasMaxLength(512);
        builder.Property(v => v.VideoUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(v => v.AntiqueId).IsUnique();

        builder.HasQueryFilter(v => !v.Antique.IsDeleted);
    }
}

public class AntiqueTypeLookupConfiguration : IEntityTypeConfiguration<AntiqueTypeLookup>
{
    public void Configure(EntityTypeBuilder<AntiqueTypeLookup> builder)
    {
        builder.ToTable("AntiqueTypes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(AntiqueModuleCatalog.Types
            .Select(entry => new AntiqueTypeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class AntiqueMaterialLookupConfiguration : IEntityTypeConfiguration<AntiqueMaterialLookup>
{
    public void Configure(EntityTypeBuilder<AntiqueMaterialLookup> builder)
    {
        builder.ToTable("AntiqueMaterials");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(AntiqueModuleCatalog.Materials
            .Select(entry => new AntiqueMaterialLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class AntiqueConditionLookupConfiguration : IEntityTypeConfiguration<AntiqueConditionLookup>
{
    public void Configure(EntityTypeBuilder<AntiqueConditionLookup> builder)
    {
        builder.ToTable("AntiqueConditions");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(AntiqueModuleCatalog.Conditions
            .Select(entry => new AntiqueConditionLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class AntiqueWorkingStatusLookupConfiguration : IEntityTypeConfiguration<AntiqueWorkingStatusLookup>
{
    public void Configure(EntityTypeBuilder<AntiqueWorkingStatusLookup> builder)
    {
        builder.ToTable("AntiqueWorkingStatuses");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(AntiqueModuleCatalog.WorkingStatuses
            .Select(entry => new AntiqueWorkingStatusLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class AntiqueOriginalityLookupConfiguration : IEntityTypeConfiguration<AntiqueOriginalityLookup>
{
    public void Configure(EntityTypeBuilder<AntiqueOriginalityLookup> builder)
    {
        builder.ToTable("AntiqueOriginalities");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(AntiqueModuleCatalog.Originalities
            .Select(entry => new AntiqueOriginalityLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
