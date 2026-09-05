using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class CoinStampConfiguration : IEntityTypeConfiguration<CoinStamp>
{
    public void Configure(EntityTypeBuilder<CoinStamp> builder)
    {
        builder.ToTable("CoinStamps");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SellerName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(20);
        builder.Property(x => x.WhatsApp).HasMaxLength(20);
        builder.Property(x => x.ItemName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.OtherType).HasMaxLength(150);
        builder.Property(x => x.Country).HasMaxLength(100);
        builder.Property(x => x.Denomination).HasMaxLength(100);
        builder.Property(x => x.OtherMetal).HasMaxLength(150);
        builder.Property(x => x.Governorate).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Center).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(300);
        builder.Property(x => x.GoogleMaps).HasMaxLength(1000);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(4000);
        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

        builder.Property(x => x.ItemType).HasConversion<int>();
        builder.Property(x => x.Metal).HasConversion<int>();
        builder.Property(x => x.Condition).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.CoinStamp)
            .HasForeignKey(i => i.CoinStampId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Video)
            .WithOne(v => v.CoinStamp)
            .HasForeignKey<CoinStampVideo>(v => v.CoinStampId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.SellerName);
        builder.HasIndex(x => x.ItemType);
        builder.HasIndex(x => x.Country);
        builder.HasIndex(x => x.IssueYear);
        builder.HasIndex(x => x.Condition);
        builder.HasIndex(x => x.IsOriginal);
        builder.HasIndex(x => x.IsRare);
        builder.HasIndex(x => x.Negotiable);
        builder.HasIndex(x => x.Center);
        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.UserId);
    }
}

public class CoinStampImageConfiguration : IEntityTypeConfiguration<CoinStampImage>
{
    public void Configure(EntityTypeBuilder<CoinStampImage> builder)
    {
        builder.ToTable("CoinStampImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.CoinStampId);

        builder.HasQueryFilter(i => !i.CoinStamp.IsDeleted);
    }
}

public class CoinStampVideoConfiguration : IEntityTypeConfiguration<CoinStampVideo>
{
    public void Configure(EntityTypeBuilder<CoinStampVideo> builder)
    {
        builder.ToTable("CoinStampVideos");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.FileName).IsRequired().HasMaxLength(256);
        builder.Property(v => v.VideoPath).IsRequired().HasMaxLength(512);
        builder.Property(v => v.VideoUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(v => v.CoinStampId).IsUnique();

        builder.HasQueryFilter(v => !v.CoinStamp.IsDeleted);
    }
}

public class CoinStampItemTypeLookupConfiguration : IEntityTypeConfiguration<CoinStampItemTypeLookup>
{
    public void Configure(EntityTypeBuilder<CoinStampItemTypeLookup> builder)
    {
        builder.ToTable("CoinStampItemTypes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(CoinStampCatalog.ItemTypes
            .Select(entry => new CoinStampItemTypeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class CoinStampMetalLookupConfiguration : IEntityTypeConfiguration<CoinStampMetalLookup>
{
    public void Configure(EntityTypeBuilder<CoinStampMetalLookup> builder)
    {
        builder.ToTable("CoinStampMetals");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(CoinStampCatalog.Metals
            .Select(entry => new CoinStampMetalLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class CoinStampConditionLookupConfiguration : IEntityTypeConfiguration<CoinStampConditionLookup>
{
    public void Configure(EntityTypeBuilder<CoinStampConditionLookup> builder)
    {
        builder.ToTable("CoinStampConditions");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(CoinStampCatalog.Conditions
            .Select(entry => new CoinStampConditionLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
