using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

internal static class CharityListingMapping
{
    public static void ApplyCommonColumns<TListing>(EntityTypeBuilder<TListing> builder)
        where TListing : CharityListing
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(20);

        builder.Property(x => x.ModerationNotes).HasMaxLength(2000);
        builder.Property(x => x.ModeratedBy).HasMaxLength(450);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        builder.HasIndex(x => new { x.ModerationStatus, x.CreatedAt });
        builder.HasIndex(x => new { x.UserId, x.CreatedAt });
        builder.HasIndex(x => x.IsDeleted);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    public static void ApplyImageColumns<TImage>(EntityTypeBuilder<TImage> builder, string table)
        where TImage : class, IOrderedListingImage
    {
        builder.ToTable(table);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileName).IsRequired().HasMaxLength(260);
        builder.Property(x => x.ImagePath).IsRequired().HasMaxLength(500);
        builder.Property(x => x.ImageUrl).IsRequired().HasMaxLength(1000);
    }
}

public class RescueConfiguration : IEntityTypeConfiguration<Rescue>
{
    public void Configure(EntityTypeBuilder<Rescue> builder)
    {
        builder.ToTable("Rescues");
        CharityListingMapping.ApplyCommonColumns(builder);

        builder.Property(x => x.RescuerName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(300);
        builder.Property(x => x.Details).IsRequired().HasMaxLength(4000);

        builder.HasMany(x => x.Images)
            .WithOne(image => image.Rescue)
            .HasForeignKey(image => image.RescueId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class RescueImageConfiguration : IEntityTypeConfiguration<RescueImage>
{
    public void Configure(EntityTypeBuilder<RescueImage> builder)
    {
        CharityListingMapping.ApplyImageColumns(builder, "RescueImages");
        builder.HasIndex(x => new { x.RescueId, x.SortOrder });
        builder.HasQueryFilter(x => !x.Rescue.IsDeleted);
    }
}

public class BloodRequestConfiguration : IEntityTypeConfiguration<BloodRequest>
{
    public void Configure(EntityTypeBuilder<BloodRequest> builder)
    {
        builder.ToTable("BloodRequests");
        CharityListingMapping.ApplyCommonColumns(builder);

        builder.Property(x => x.RequesterName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.BloodGroup).HasConversion<int>();
        builder.Property(x => x.Governorate).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Center).IsRequired().HasMaxLength(100);
        builder.Property(x => x.HospitalName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(300);
        builder.Property(x => x.Details).IsRequired().HasMaxLength(4000);

        builder.HasIndex(x => new { x.BloodGroup, x.Center, x.CreatedAt });

        builder.HasMany(x => x.Images)
            .WithOne(image => image.BloodRequest)
            .HasForeignKey(image => image.BloodRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class BloodRequestImageConfiguration : IEntityTypeConfiguration<BloodRequestImage>
{
    public void Configure(EntityTypeBuilder<BloodRequestImage> builder)
    {
        CharityListingMapping.ApplyImageColumns(builder, "BloodRequestImages");
        builder.HasIndex(x => new { x.BloodRequestId, x.SortOrder });
        builder.HasQueryFilter(x => !x.BloodRequest.IsDeleted);
    }
}

public class AskConsultConfiguration : IEntityTypeConfiguration<AskConsult>
{
    public void Configure(EntityTypeBuilder<AskConsult> builder)
    {
        builder.ToTable("AskConsults");
        CharityListingMapping.ApplyCommonColumns(builder);

        builder.Property(x => x.Category).HasConversion<int>();
        builder.Property(x => x.OtherCategory).HasMaxLength(150);
        builder.Property(x => x.AskerName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Question).IsRequired().HasMaxLength(4000);
        builder.Property(x => x.Governorate).HasMaxLength(100);
        builder.Property(x => x.Center).HasMaxLength(100);

        builder.HasIndex(x => new { x.Category, x.CreatedAt });

        builder.HasMany(x => x.Images)
            .WithOne(image => image.AskConsult)
            .HasForeignKey(image => image.AskConsultId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Likes)
            .WithOne(like => like.AskConsult)
            .HasForeignKey(like => like.AskConsultId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Comments)
            .WithOne(comment => comment.AskConsult)
            .HasForeignKey(comment => comment.AskConsultId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class AskConsultImageConfiguration : IEntityTypeConfiguration<AskConsultImage>
{
    public void Configure(EntityTypeBuilder<AskConsultImage> builder)
    {
        CharityListingMapping.ApplyImageColumns(builder, "AskConsultImages");
        builder.HasIndex(x => new { x.AskConsultId, x.SortOrder });
        builder.HasQueryFilter(x => !x.AskConsult.IsDeleted);
    }
}

public class AskConsultLikeConfiguration : IEntityTypeConfiguration<AskConsultLike>
{
    public void Configure(EntityTypeBuilder<AskConsultLike> builder)
    {
        builder.ToTable("AskConsultLikes");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();

        builder.HasIndex(x => new { x.AskConsultId, x.UserId })
            .IsUnique()
            .HasDatabaseName("IX_AskConsultLikes_AskConsult_User");

        builder.HasQueryFilter(x => !x.AskConsult.IsDeleted);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public class AskConsultCommentConfiguration : IEntityTypeConfiguration<AskConsultComment>
{
    public void Configure(EntityTypeBuilder<AskConsultComment> builder)
    {
        builder.ToTable("AskConsultComments");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Comment).IsRequired().HasMaxLength(CharityCatalog.MaxCommentLength);

        builder.HasIndex(x => new { x.AskConsultId, x.CreatedAt });
        builder.HasQueryFilter(x => !x.AskConsult.IsDeleted);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
