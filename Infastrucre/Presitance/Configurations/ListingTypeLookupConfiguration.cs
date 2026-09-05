using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;
using Shared.Enums;

namespace Persistence.Configurations;

public class ListingTypeLookupConfiguration : IEntityTypeConfiguration<ListingTypeLookup>
{
    public void Configure(EntityTypeBuilder<ListingTypeLookup> builder)
    {
        builder.ToTable("ListingTypes");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).ValueGeneratedNever();
        builder.Property(l => l.Name).IsRequired().HasMaxLength(100);
        builder.HasIndex(l => l.Name).IsUnique();

        builder.HasData(AdvertisementCatalog.ListingTypeNames
            .Select(entry => new ListingTypeLookup { Id = (int)entry.Key, Name = entry.Value })
            .ToArray());
    }
}
