using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class ExperienceLevelLookupConfiguration : IEntityTypeConfiguration<ExperienceLevelLookup>
{
    public void Configure(EntityTypeBuilder<ExperienceLevelLookup> builder)
    {
        builder.ToTable("ExperienceLevels");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.HasIndex(e => e.Name).IsUnique();

        builder.HasData(WorkshopCraftsmenCatalog.ExperienceLevelNames
            .Select(kvp => new ExperienceLevelLookup { Id = (int)kvp.Key, Name = kvp.Value }));
    }
}
