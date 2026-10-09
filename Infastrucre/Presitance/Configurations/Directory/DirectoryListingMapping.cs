using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

internal static class DirectoryListingMapping
{
    public static void Apply<TListing, TImage>(EntityTypeBuilder<TListing> builder, string table)
        where TListing : DirectoryListing<TImage>
        where TImage : DirectoryListingImage
    {
        builder.ToTable(table);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(DirectoryListingRules.MaxNameLength);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(DirectoryListingRules.MaxTitleLength);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(DirectoryListingRules.MaxDescriptionLength);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(DirectoryListingRules.MaxAddressLength);
        builder.Property(x => x.GoogleMapsUrl).HasMaxLength(DirectoryListingRules.MaxGoogleMapsUrlLength);
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(DirectoryListingRules.MaxPhoneLength);
        builder.Property(x => x.WhatsApp).HasMaxLength(DirectoryListingRules.MaxPhoneLength);
        builder.Property(x => x.Email).HasMaxLength(DirectoryListingRules.MaxEmailLength);

        builder.Property(x => x.ModerationNotes).HasMaxLength(2000);
        builder.Property(x => x.ModeratedBy).HasMaxLength(450);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        builder.HasIndex(x => new { x.UserId, x.CreatedAt });
        builder.HasIndex(x => x.IsDeleted);

        builder.HasIndex(x => new { x.ModerationStatus, x.Name }, $"IX_{table}_Advertisers")
            .IncludeProperties(x => new { x.IsDeleted, x.ExpireAt });

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    public static void ApplyImage<TImage>(EntityTypeBuilder<TImage> builder, string table)
        where TImage : DirectoryListingImage
    {
        builder.ToTable(table);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileName).IsRequired().HasMaxLength(260);
        builder.Property(x => x.ImagePath).IsRequired().HasMaxLength(500);
        builder.Property(x => x.ImageUrl).IsRequired().HasMaxLength(1000);

        builder.HasIndex(x => new { x.ListingId, x.SortOrder });
    }
}
