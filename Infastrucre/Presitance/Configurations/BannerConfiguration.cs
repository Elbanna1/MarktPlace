using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class BannerConfiguration : IEntityTypeConfiguration<Banner>
{
    public void Configure(EntityTypeBuilder<Banner> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title).HasMaxLength(BannerCatalog.MaxTitleLength);
        builder.Property(b => b.Description).HasMaxLength(BannerCatalog.MaxDescriptionLength);
        builder.Property(b => b.RedirectUrl).HasMaxLength(BannerCatalog.MaxRedirectUrlLength);

        builder.Property(b => b.ImageFileName).IsRequired().HasMaxLength(260);
        builder.Property(b => b.ImagePath).IsRequired().HasMaxLength(500);
        builder.Property(b => b.ImageUrl).IsRequired().HasMaxLength(1000);

        builder.Property(b => b.IsActive).HasDefaultValue(true);

        builder.HasIndex(b => new { b.IsActive, b.DisplayOrder });

        builder.HasIndex(b => new { b.StartDate, b.EndDate });
    }
}
