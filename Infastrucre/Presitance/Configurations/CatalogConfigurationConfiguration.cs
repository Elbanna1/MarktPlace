using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class CatalogConfigurationConfiguration : IEntityTypeConfiguration<CatalogConfiguration>
{
    public void Configure(EntityTypeBuilder<CatalogConfiguration> builder)
    {
        builder.ToTable("CatalogConfigurations", table =>
        {
            table.HasCheckConstraint("CK_CatalogConfigurations_Price", "[Price] IS NULL OR [Price] > 0");
            table.HasCheckConstraint(
                "CK_CatalogConfigurations_Window",
                "[ActiveFrom] IS NULL OR [ActiveUntil] IS NULL OR [ActiveFrom] < [ActiveUntil]");
            table.HasCheckConstraint(
                "CK_CatalogConfigurations_Area",
                "([Kind] = 1 AND [Area] = 0) OR ([Kind] = 2 AND [Area] > 0)");
        });

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Price).HasColumnType("decimal(18,2)");
        builder.Property(c => c.Currency).HasMaxLength(3);
        builder.Property(c => c.CreatedBy).HasMaxLength(450);
        builder.Property(c => c.UpdatedBy).HasMaxLength(450);

        builder.HasIndex(c => new { c.Kind, c.NodeType, c.NodeId, c.Area })
            .IsUnique()
            .HasDatabaseName("UX_CatalogConfigurations_Node");
    }
}
