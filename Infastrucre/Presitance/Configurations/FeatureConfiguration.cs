using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class FeatureConfiguration : IEntityTypeConfiguration<Feature>
{
    public void Configure(EntityTypeBuilder<Feature> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).ValueGeneratedNever();
        builder.Property(f => f.Name).IsRequired().HasMaxLength(100);
        builder.Property(f => f.Group).IsRequired().HasMaxLength(50);
        builder.HasIndex(f => f.Name).IsUnique();

        var seed = CarCatalog.Features
            .Select(feature => new Feature
            {
                Id = feature.Id,
                Name = feature.Name,
                Group = feature.Group,
                Scope = (int)feature.Scope
            })
            .ToArray();

        builder.HasData(seed);
    }
}
