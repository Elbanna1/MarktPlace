using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class JobOpportunityConfiguration : IEntityTypeConfiguration<JobOpportunity>
{
    public void Configure(EntityTypeBuilder<JobOpportunity> builder)
    {
        builder.HasKey(j => j.Id);

        builder.Property(j => j.EmployerName).IsRequired().HasMaxLength(150);
        builder.Property(j => j.Phone).IsRequired().HasMaxLength(20);
        builder.Property(j => j.WhatsApp).IsRequired().HasMaxLength(20);
        builder.Property(j => j.JobTitle).IsRequired().HasMaxLength(150);
        builder.Property(j => j.OtherJobField).HasMaxLength(150);
        builder.Property(j => j.Governorate).IsRequired().HasMaxLength(100);

        builder.Property(j => j.Center).HasMaxLength(100);
        builder.Property(j => j.Address).IsRequired().HasMaxLength(300);
        builder.Property(j => j.GoogleMaps).HasMaxLength(1000);
        builder.Property(j => j.Title).IsRequired().HasMaxLength(150);
        builder.Property(j => j.Description).IsRequired().HasMaxLength(4000);
        builder.Property(j => j.UserId).IsRequired();

        builder.Property(j => j.Salary).HasColumnType("decimal(18,2)");

        builder.Property(j => j.LogoPath).HasMaxLength(500);
        builder.Property(j => j.LogoUrl).HasMaxLength(1000);

        builder.Property(j => j.JobField).HasConversion<int>();
        builder.Property(j => j.RequiredExperience).HasConversion<int>();
        builder.Property(j => j.WorkType).HasConversion<int>();
        builder.Property(j => j.SalaryType).HasConversion<int>();

        builder.HasOne(j => j.User)
            .WithMany()
            .HasForeignKey(j => j.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(j => j.Images)
            .WithOne(i => i.JobOpportunity)
            .HasForeignKey(i => i.JobOpportunityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, j => !j.IsDeleted);

        builder.HasIndex(j => j.CreatedAt);
        builder.HasIndex(j => j.JobTitle);
        builder.HasIndex(j => j.JobField);
        builder.HasIndex(j => j.WorkType);
        builder.HasIndex(j => j.RequiredExperience);
        builder.HasIndex(j => j.Center);
        builder.HasIndex(j => j.UserId);
    }
}

public class JobOpportunityImageConfiguration : IEntityTypeConfiguration<JobOpportunityImage>
{
    public void Configure(EntityTypeBuilder<JobOpportunityImage> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(260);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(500);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1000);

        builder.HasIndex(i => i.JobOpportunityId);
    }
}
