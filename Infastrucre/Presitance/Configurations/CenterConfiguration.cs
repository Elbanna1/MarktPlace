using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class CenterConfiguration : IEntityTypeConfiguration<Center>
{
    public void Configure(EntityTypeBuilder<Center> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

        builder.Property(c => c.IsActive).IsRequired().HasDefaultValue(true);

        builder.Property(c => c.SortOrder).IsRequired().HasDefaultValue(0);

        builder.HasOne(c => c.Governorate)
            .WithMany(g => g.Centers)
            .HasForeignKey(c => c.GovernorateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => new { c.GovernorateId, c.Name }).IsUnique();

        var seed = LocationConstants.Centers
            .Select((name, index) => new Center { Id = index + 1, GovernorateId = 1, Name = name })
            .ToArray();

        builder.HasData(seed);
    }
}
