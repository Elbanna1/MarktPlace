using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class GovernorateConfiguration : IEntityTypeConfiguration<Governorate>
{
    public void Configure(EntityTypeBuilder<Governorate> builder)
    {
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id).ValueGeneratedNever();
        builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
        builder.HasIndex(g => g.Name).IsUnique();

        builder.Property(g => g.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(g => g.SortOrder).IsRequired().HasDefaultValue(0);

        builder.HasData(new Governorate { Id = 1, Name = LocationConstants.Governorate });
    }
}
