using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public abstract class HomeFurnishingLookupConfiguration<TLookup, TValue> : IEntityTypeConfiguration<TLookup>
    where TLookup : class, IHomeFurnishingLookup, new()
    where TValue : struct, Enum
{
    protected abstract string TableName { get; }

    protected abstract IReadOnlyList<HomeFurnishingLookupEntry<TValue>> Entries { get; }

    public void Configure(EntityTypeBuilder<TLookup> builder)
    {
        builder.ToTable(TableName);

        builder.HasKey(nameof(IHomeFurnishingLookup.Id));
        builder.Property(nameof(IHomeFurnishingLookup.Id)).ValueGeneratedNever();
        builder.Property(nameof(IHomeFurnishingLookup.Name)).IsRequired().HasMaxLength(150);
        builder.Property(nameof(IHomeFurnishingLookup.NameEn)).IsRequired().HasMaxLength(150);

        builder.HasIndex(nameof(IHomeFurnishingLookup.Name)).IsUnique();

        builder.HasData(Entries.Select(entry => new TLookup
        {
            Id = Convert.ToInt32(entry.Value),
            Name = entry.Name,
            NameEn = entry.NameEn
        }));
    }
}

public abstract class HomeFurnishingImageConfiguration<TImage> : IEntityTypeConfiguration<TImage>
    where TImage : class, IOrderedListingImage
{
    protected abstract string TableName { get; }

    protected abstract string ListingIdPropertyName { get; }

    protected abstract void ApplySoftDeleteFilter(EntityTypeBuilder<TImage> builder);

    public void Configure(EntityTypeBuilder<TImage> builder)
    {
        builder.ToTable(TableName);

        builder.HasKey(nameof(IOrderedListingImage.Id));

        builder.Property(nameof(IOrderedListingImage.FileName)).IsRequired().HasMaxLength(256);
        builder.Property(nameof(IOrderedListingImage.ImagePath)).IsRequired().HasMaxLength(512);
        builder.Property(nameof(IOrderedListingImage.ImageUrl)).IsRequired().HasMaxLength(1024);

        builder.HasIndex(ListingIdPropertyName, nameof(IOrderedListingImage.SortOrder));

        ApplySoftDeleteFilter(builder);
    }
}

internal static class HomeFurnishingListingMapping
{
    public static void ApplyCommonColumns<TListing>(EntityTypeBuilder<TListing> builder)
        where TListing : class
    {
        builder.Property("SellerName").IsRequired().HasMaxLength(150);
        builder.Property("Phone").IsRequired().HasMaxLength(20);
        builder.Property("WhatsApp").HasMaxLength(20);
        builder.Property("Email").HasMaxLength(256);

        builder.Property("ProductName").IsRequired().HasMaxLength(150);

        builder.Property("Governorate").IsRequired().HasMaxLength(100);
        builder.Property("Center").IsRequired().HasMaxLength(100);
        builder.Property("Address").IsRequired().HasMaxLength(300);
        builder.Property("GoogleMaps").HasMaxLength(1000);

        builder.Property("Title").IsRequired().HasMaxLength(150);
        builder.Property("Description").IsRequired().HasMaxLength(4000);

        builder.Property("UserId").IsRequired();

        builder.Property("Price").HasColumnType("decimal(18,2)");

        builder.Property("VideoPath").HasMaxLength(500);
        builder.Property("VideoUrl").HasMaxLength(1000);
    }

    public static void ApplyCommonIndexes<TListing>(EntityTypeBuilder<TListing> builder)
        where TListing : class
    {
        builder.HasIndex("CreatedAt");
        builder.HasIndex("ProductName");
        builder.HasIndex("Center");
        builder.HasIndex("Price");
        builder.HasIndex("UserId");
        builder.HasIndex("ViewCount");

        builder.HasIndex("IsPremium", "IsFeatured");
    }
}
