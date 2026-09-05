using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public abstract class RealEstateLookupConfiguration<TLookup, TValue> : IEntityTypeConfiguration<TLookup>
    where TLookup : class, IRealEstateLookup, new()
    where TValue : struct, Enum
{
    protected abstract string TableName { get; }

    protected abstract IReadOnlyList<RealEstateLookupEntry<TValue>> Entries { get; }

    public void Configure(EntityTypeBuilder<TLookup> builder)
    {
        builder.ToTable(TableName);

        builder.HasKey(nameof(IRealEstateLookup.Id));
        builder.Property(nameof(IRealEstateLookup.Id)).ValueGeneratedNever();
        builder.Property(nameof(IRealEstateLookup.Name)).IsRequired().HasMaxLength(150);
        builder.Property(nameof(IRealEstateLookup.NameEn)).IsRequired().HasMaxLength(150);

        builder.HasIndex(nameof(IRealEstateLookup.Name)).IsUnique();

        builder.HasData(Entries.Select(entry => new TLookup
        {
            Id = Convert.ToInt32(entry.Value),
            Name = entry.Name,
            NameEn = entry.NameEn
        }));
    }
}

public abstract class RealEstateImageConfiguration<TImage> : IEntityTypeConfiguration<TImage>
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

internal static class RealEstateListingMapping
{
    public static void ApplyCommonColumns<TListing>(EntityTypeBuilder<TListing> builder)
        where TListing : class
    {
        builder.Property("Title").IsRequired().HasMaxLength(150);
        builder.Property("Description").IsRequired().HasMaxLength(4000);
        builder.Property("AdvertiserName").IsRequired().HasMaxLength(150);

        builder.Property("Governorate").IsRequired().HasMaxLength(100);
        builder.Property("Center").IsRequired().HasMaxLength(100);
        builder.Property("OtherProject").HasMaxLength(150);
        builder.Property("District").HasMaxLength(150);
        builder.Property("Address").IsRequired().HasMaxLength(300);
        builder.Property("GoogleMaps").HasMaxLength(1000);

        builder.Property("Phone").IsRequired().HasMaxLength(20);
        builder.Property("WhatsApp").HasMaxLength(20);
        builder.Property("Email").HasMaxLength(256);

        builder.Property("Notes").HasMaxLength(4000);

        builder.Property("UserId").IsRequired();

        builder.Property("ListingType").HasConversion<int>();
        builder.Property("Project").HasConversion<int>();

        builder.Property("VideoPath").HasMaxLength(500);
        builder.Property("VideoUrl").HasMaxLength(1000);
    }

    public static void ApplyLicenceColumns<TListing>(EntityTypeBuilder<TListing> builder)
        where TListing : class
    {
        builder.Property("LicenseNumber").HasMaxLength(100);
        builder.Property("LicenseIssuer").HasMaxLength(150);
    }

    public static void ApplyExchangeColumns<TListing>(EntityTypeBuilder<TListing> builder)
        where TListing : class
    {
        builder.Property("DifferenceAmount").HasColumnType("decimal(18,2)");
        builder.Property("ExchangeDetails").HasMaxLength(4000);
    }

    public static void ApplyRentColumns<TListing>(EntityTypeBuilder<TListing> builder)
        where TListing : class
    {
        builder.Property("RentValue").HasColumnType("decimal(18,2)");
        builder.Property("SecurityDeposit").HasColumnType("decimal(18,2)");
        builder.Property("OwnerConditions").HasMaxLength(4000);
    }

    public static void ApplyCommonIndexes<TListing>(EntityTypeBuilder<TListing> builder)
        where TListing : class
    {
        builder.HasIndex("CreatedAt");
        builder.HasIndex("Title");
        builder.HasIndex("ListingType");
        builder.HasIndex("Center");
        builder.HasIndex("Project");
        builder.HasIndex("UserId");
        builder.HasIndex("ViewCount");
        builder.HasIndex("Negotiable");

        builder.HasIndex("IsPremium", "IsFeatured");
    }
}
