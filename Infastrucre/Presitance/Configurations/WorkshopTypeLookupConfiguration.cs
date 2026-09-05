using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class WorkshopTypeLookupConfiguration : IEntityTypeConfiguration<WorkshopTypeLookup>
{
    public void Configure(EntityTypeBuilder<WorkshopTypeLookup> builder)
    {
        builder.ToTable("WorkshopTypes");

        builder.HasKey(w => w.Id);
        builder.Property(w => w.Id).ValueGeneratedNever();
        builder.Property(w => w.Name).IsRequired().HasMaxLength(150);
        builder.HasIndex(w => w.Name).IsUnique();

        builder.HasData(WorkshopCraftsmenCatalog.WorkshopTypeNames
            .Select(kvp => new WorkshopTypeLookup { Id = (int)kvp.Key, Name = kvp.Value }));
    }
}
