using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class JobRequestConfiguration : IEntityTypeConfiguration<JobRequest>
{
    public void Configure(EntityTypeBuilder<JobRequest> builder)
    {
        builder.HasKey(j => j.Id);

        builder.Property(j => j.ApplicantName).IsRequired().HasMaxLength(150);
        builder.Property(j => j.Phone).IsRequired().HasMaxLength(20);
        builder.Property(j => j.WhatsApp).IsRequired().HasMaxLength(20);
        builder.Property(j => j.OtherJobField).HasMaxLength(150);
        builder.Property(j => j.Skills).IsRequired().HasMaxLength(1000);
        builder.Property(j => j.Governorate).IsRequired().HasMaxLength(100);

        builder.Property(j => j.Center).HasMaxLength(100);
        builder.Property(j => j.Address).IsRequired().HasMaxLength(300);
        builder.Property(j => j.Title).IsRequired().HasMaxLength(150);
        builder.Property(j => j.Description).IsRequired().HasMaxLength(4000);
        builder.Property(j => j.UserId).IsRequired();

        builder.Property(j => j.ProfileImagePath).HasMaxLength(500);
        builder.Property(j => j.ProfileImageUrl).HasMaxLength(1000);
        builder.Property(j => j.CvFilePath).HasMaxLength(500);
        builder.Property(j => j.CvFileUrl).HasMaxLength(1000);
        builder.Property(j => j.CvFileName).HasMaxLength(260);
        builder.Property(j => j.IntroVideoPath).HasMaxLength(500);
        builder.Property(j => j.IntroVideoUrl).HasMaxLength(1000);

        builder.Property(j => j.JobField).HasConversion<int>();
        builder.Property(j => j.Experience).HasConversion<int>();
        builder.Property(j => j.Education).HasConversion<int>();

        builder.HasOne(j => j.User)
            .WithMany()
            .HasForeignKey(j => j.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, j => !j.IsDeleted);

        builder.HasIndex(j => j.CreatedAt);
        builder.HasIndex(j => j.JobField);
        builder.HasIndex(j => j.Experience);
        builder.HasIndex(j => j.Education);
        builder.HasIndex(j => j.Center);
        builder.HasIndex(j => j.UserId);
    }
}
