using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class SupplierSpecializationLookupConfiguration : IEntityTypeConfiguration<SupplierSpecializationLookup>
{
    public void Configure(EntityTypeBuilder<SupplierSpecializationLookup> builder)
    {
        builder.ToTable("SupplierSpecializations");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedNever();
        builder.Property(s => s.Group).IsRequired().HasMaxLength(100);
        builder.Property(s => s.GroupAr).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(150);
        builder.Property(s => s.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(s => new { s.Group, s.Name }).IsUnique();

        builder.HasData(SupplierCatalog.Specializations
            .Select(entry => new SupplierSpecializationLookup
            {
                Id = (int)entry.Value,
                Group = entry.Group,
                GroupAr = entry.GroupAr,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
