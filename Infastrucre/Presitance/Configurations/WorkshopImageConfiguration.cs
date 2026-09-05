using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class WorkshopImageConfiguration : IEntityTypeConfiguration<WorkshopImage>
{
    public void Configure(EntityTypeBuilder<WorkshopImage> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.WorkshopId);

        builder.HasQueryFilter(i => !i.Workshop.IsDeleted);
    }
}
