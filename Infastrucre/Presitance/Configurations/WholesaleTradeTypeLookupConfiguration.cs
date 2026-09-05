using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class WholesaleTradeTypeLookupConfiguration : IEntityTypeConfiguration<WholesaleTradeTypeLookup>
{
    public void Configure(EntityTypeBuilder<WholesaleTradeTypeLookup> builder)
    {
        builder.ToTable("WholesaleTradeTypes");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();
        builder.Property(t => t.Name).IsRequired().HasMaxLength(150);
        builder.Property(t => t.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(t => t.Name).IsUnique();

        builder.HasData(WholesaleTraderCatalog.TradeTypes
            .Select(entry => new WholesaleTradeTypeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
