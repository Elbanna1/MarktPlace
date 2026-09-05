using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class PlatformSettingConfiguration : IEntityTypeConfiguration<PlatformSetting>
{
    public const int SingletonId = 1;

    public void Configure(EntityTypeBuilder<PlatformSetting> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedNever();

        builder.ToTable(table => table.HasCheckConstraint(
            "CK_PlatformSettings_SingleRow", $"[Id] = {SingletonId}"));

        builder.Property(s => s.SiteName).IsRequired().HasMaxLength(150);
        builder.Property(s => s.SiteNameEn).HasMaxLength(150);
        builder.Property(s => s.Description).HasMaxLength(1000);

        builder.Property(s => s.LogoUrl).HasMaxLength(1000);
        builder.Property(s => s.LogoPath).HasMaxLength(500);
        builder.Property(s => s.FaviconUrl).HasMaxLength(1000);
        builder.Property(s => s.FaviconPath).HasMaxLength(500);

        builder.Property(s => s.PhoneNumber).HasMaxLength(30);
        builder.Property(s => s.WhatsAppNumber).HasMaxLength(30);
        builder.Property(s => s.Email).HasMaxLength(256);
        builder.Property(s => s.Address).HasMaxLength(500);

        builder.Property(s => s.FacebookUrl).HasMaxLength(500);
        builder.Property(s => s.InstagramUrl).HasMaxLength(500);
        builder.Property(s => s.TelegramUrl).HasMaxLength(500);
        builder.Property(s => s.TwitterUrl).HasMaxLength(500);
        builder.Property(s => s.YouTubeUrl).HasMaxLength(500);
        builder.Property(s => s.TikTokUrl).HasMaxLength(500);
        builder.Property(s => s.LinkedInUrl).HasMaxLength(500);

        builder.Property(s => s.MaintenanceMode).IsRequired().HasDefaultValue(false);
        builder.Property(s => s.MaintenanceMessage).HasMaxLength(1000);

        builder.Property(s => s.UpdatedBy).HasMaxLength(450);

        builder.HasData(new PlatformSetting
        {
            Id = SingletonId,
            SiteName = "شبيك لبيك",
            SiteNameEn = "Shobik Lobik",
            Description = "سوق الفيوم الإلكتروني",
            MaintenanceMode = false
        });
    }
}
