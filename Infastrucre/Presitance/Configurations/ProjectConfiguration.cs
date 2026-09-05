using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.Property(p => p.Name).IsRequired().HasMaxLength(150);
        builder.Property(p => p.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(p => p.SortOrder).IsRequired().HasDefaultValue(0);

        builder.HasOne(p => p.Center)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.CenterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => new { p.CenterId, p.Name }).IsUnique();

        var newFayoumCenterId = LocationConstants.Centers.ToList().IndexOf("الفيوم الجديدة") + 1;

        if (newFayoumCenterId > 0)
        {
            builder.HasData(new Project
            {
                Id = 1,
                CenterId = newFayoumCenterId,
                Name = "ابني بيتك",
                IsActive = true,
                SortOrder = 0
            });
        }
    }
}
