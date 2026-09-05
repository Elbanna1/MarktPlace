using Domain.Entities.Listings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations.Listings;

public class ListingViewCounterConfiguration : IEntityTypeConfiguration<ListingViewCounter>
{
    public void Configure(EntityTypeBuilder<ListingViewCounter> builder)
    {
        builder.ToTable("ListingViewCounters");

        builder.HasKey(c => new { c.ListingType, c.ListingId });

        builder.Property(c => c.ListingType).HasConversion<int>();
        builder.Property(c => c.TotalViews).HasDefaultValue(0);
    }
}

public class ListingViewerConfiguration : IEntityTypeConfiguration<ListingViewer>
{
    public void Configure(EntityTypeBuilder<ListingViewer> builder)
    {
        builder.ToTable("ListingViewers");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.ListingType).HasConversion<int>();

        builder.Property(v => v.ViewerKey)
            .IsRequired()
            .HasMaxLength(ListingInteractionCatalog.ViewerKeyMaxLength);

        builder.Property(v => v.ViewCount).HasDefaultValue(0);

        builder.HasIndex(v => new { v.ListingType, v.ListingId, v.ViewerKey })
            .IsUnique()
            .HasDatabaseName("IX_ListingViewers_Listing_Viewer");

        builder.HasIndex(v => new { v.UserId, v.LastViewedAt })
            .HasDatabaseName("IX_ListingViewers_User_LastViewedAt")
            .HasFilter("[UserId] IS NOT NULL");

        builder.HasOne(v => v.User)
            .WithMany()
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public class ListingFavoriteConfiguration : IEntityTypeConfiguration<ListingFavorite>
{
    public void Configure(EntityTypeBuilder<ListingFavorite> builder)
    {
        builder.ToTable("ListingFavorites");

        builder.HasKey(f => new { f.UserId, f.ListingType, f.ListingId });

        builder.Property(f => f.ListingType).HasConversion<int>();

        builder.HasIndex(f => new { f.ListingType, f.ListingId })
            .HasDatabaseName("IX_ListingFavorites_Listing");

        builder.HasIndex(f => new { f.UserId, f.CreatedAt })
            .HasDatabaseName("IX_ListingFavorites_User_CreatedAt");

        builder.HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public class ListingRatingConfiguration : IEntityTypeConfiguration<ListingRating>
{
    public void Configure(EntityTypeBuilder<ListingRating> builder)
    {
        builder.ToTable("ListingRatings");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.ListingType).HasConversion<int>();

        builder.Property(r => r.ListingTitle).HasMaxLength(300);

        builder.HasIndex(r => new { r.ListingType, r.ListingId, r.ReviewerUserId })
            .IsUnique()
            .HasDatabaseName("IX_ListingRatings_Listing_Reviewer");

        builder.HasIndex(r => new { r.ListingType, r.ListingId })
            .HasDatabaseName("IX_ListingRatings_Listing");

        builder.HasIndex(r => new { r.ReviewerUserId, r.CreatedAt })
            .HasDatabaseName("IX_ListingRatings_Reviewer_CreatedAt");

        builder.ToTable(table => table.HasCheckConstraint(
            "CK_ListingRatings_Rating",
            $"[Rating] BETWEEN {ListingInteractionCatalog.MinRating} AND {ListingInteractionCatalog.MaxRating}"));

        builder.HasOne(r => r.Reviewer)
            .WithMany()
            .HasForeignKey(r => r.ReviewerUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public class ListingReportConfiguration : IEntityTypeConfiguration<ListingReport>
{
    public void Configure(EntityTypeBuilder<ListingReport> builder)
    {
        builder.ToTable("ListingReports");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.ListingType).HasConversion<int>();
        builder.Property(r => r.Reason).HasConversion<int>();
        builder.Property(r => r.Status).HasConversion<int>();

        builder.Property(r => r.Details)
            .HasMaxLength(ListingInteractionCatalog.ReportDetailsMaxLength);

        builder.Property(r => r.AdminNote)
            .HasMaxLength(ListingInteractionCatalog.ReportAdminNoteMaxLength);

        builder.Property(r => r.ListingTitle).HasMaxLength(300);

        builder.HasIndex(r => new { r.ListingType, r.ListingId, r.ReporterUserId })
            .IsUnique()
            .HasDatabaseName("IX_ListingReports_Listing_Reporter");

        builder.HasIndex(r => new { r.Status, r.CreatedAt })
            .HasDatabaseName("IX_ListingReports_Status_CreatedAt");

        builder.HasIndex(r => new { r.ListingType, r.ListingId })
            .HasDatabaseName("IX_ListingReports_Listing");

        builder.HasOne(r => r.Reporter)
            .WithMany()
            .HasForeignKey(r => r.ReporterUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
