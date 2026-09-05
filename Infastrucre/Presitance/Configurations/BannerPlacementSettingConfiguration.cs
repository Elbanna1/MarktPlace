using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class BannerPlacementSettingConfiguration : IEntityTypeConfiguration<BannerPlacementSetting>
{
    public void Configure(EntityTypeBuilder<BannerPlacementSetting> builder)
    {
        builder.ToTable("BannerPlacementSettings");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Location).HasConversion<int>();

        builder.Property(p => p.Price).HasColumnType("decimal(18,2)");

        builder.Property(p => p.AllowedFormats).IsRequired().HasMaxLength(200);

        builder.HasIndex(p => p.Location).IsUnique();

        builder.HasIndex(p => new { p.IsActive, p.DisplayOrder });

        builder.HasData(BannerBookingCatalog.SeedPlacements.Select(seed => new BannerPlacementSetting
        {
            Id = seed.Id,
            Location = seed.Location,
            Price = seed.Price,
            MaxSlots = seed.MaxSlots,
            DesktopWidth = seed.DesktopWidth,
            DesktopHeight = seed.DesktopHeight,
            MobileWidth = seed.MobileWidth,
            MobileHeight = seed.MobileHeight,
            MaxImageSizeBytes = seed.MaxImageSizeBytes,
            AllowedFormats = seed.AllowedFormats,
            DurationDays = seed.DurationDays,
            IsActive = true,
            DisplayOrder = seed.DisplayOrder,
            CreatedAt = BannerBookingCatalog.SeedTimestamp
        }));
    }
}
