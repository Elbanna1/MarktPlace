using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class CraftsmanSpecializationLookupConfiguration : IEntityTypeConfiguration<CraftsmanSpecializationLookup>
{
    public void Configure(EntityTypeBuilder<CraftsmanSpecializationLookup> builder)
    {
        builder.ToTable("CraftsmanSpecializations");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedNever();
        builder.Property(s => s.GroupName).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(150);

        builder.HasData(WorkshopCraftsmenCatalog.Specializations
            .Select(s => new CraftsmanSpecializationLookup
            {
                Id = (int)s.Value,
                GroupName = s.Group,
                Name = s.Name
            }));
    }
}
