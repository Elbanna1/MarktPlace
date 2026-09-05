using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class MerchantSaleTypeLookupConfiguration : IEntityTypeConfiguration<MerchantSaleTypeLookup>
{
    public void Configure(EntityTypeBuilder<MerchantSaleTypeLookup> builder)
    {
        builder.ToTable("MerchantSaleTypes");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedNever();
        builder.Property(s => s.Name).IsRequired().HasMaxLength(150);
        builder.Property(s => s.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(s => s.Name).IsUnique();

        builder.HasData(FruitVegetableMerchantCatalog.SaleTypeNames
            .Select(entry => new MerchantSaleTypeLookup
            {
                Id = (int)entry.Key,
                Name = entry.Value,
                NameEn = FruitVegetableMerchantCatalog.SaleTypeNamesEn[entry.Key]
            }));
    }
}
