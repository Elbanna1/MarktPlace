using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;
using Shared.Enums;

namespace Persistence.Configurations;

public class ProductionSpecialtyLookupConfiguration : IEntityTypeConfiguration<ProductionSpecialtyLookup>
{
    public void Configure(EntityTypeBuilder<ProductionSpecialtyLookup> builder)
    {
        builder.ToTable("ProductionSpecialties");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();
        builder.Property(p => p.Name).IsRequired().HasMaxLength(150);
        builder.HasIndex(p => p.Name).IsUnique();

        builder.HasData(BusinessCatalog.ProductionSpecialtyNames
            .Select(entry => new ProductionSpecialtyLookup { Id = (int)entry.Key, Name = entry.Value }));
    }
}

public class FarmTypeLookupConfiguration : IEntityTypeConfiguration<FarmTypeLookup>
{
    public void Configure(EntityTypeBuilder<FarmTypeLookup> builder)
    {
        builder.ToTable("FarmTypes");

        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).ValueGeneratedNever();
        builder.Property(f => f.Name).IsRequired().HasMaxLength(150);
        builder.Property(f => f.Group).IsRequired().HasMaxLength(100);
        builder.Property(f => f.GroupAr).IsRequired().HasMaxLength(100);
        builder.HasIndex(f => f.Name).IsUnique();

        builder.HasData(BusinessCatalog.FarmTypes
            .Select(entry => new FarmTypeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                Group = entry.Group,
                GroupAr = entry.GroupAr
            }));
    }
}

public class AvailabilitySeasonLookupConfiguration : IEntityTypeConfiguration<AvailabilitySeasonLookup>
{
    public void Configure(EntityTypeBuilder<AvailabilitySeasonLookup> builder)
    {
        builder.ToTable("AvailabilitySeasons");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();
        builder.Property(a => a.Name).IsRequired().HasMaxLength(150);
        builder.HasIndex(a => a.Name).IsUnique();

        builder.HasData(BusinessCatalog.AvailabilitySeasonNames
            .Select(entry => new AvailabilitySeasonLookup { Id = (int)entry.Key, Name = entry.Value }));
    }
}

public class FarmingMethodLookupConfiguration : IEntityTypeConfiguration<FarmingMethodLookup>
{
    public void Configure(EntityTypeBuilder<FarmingMethodLookup> builder)
    {
        builder.ToTable("FarmingMethods");

        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).ValueGeneratedNever();
        builder.Property(f => f.Name).IsRequired().HasMaxLength(150);
        builder.Property(f => f.Code).IsRequired().HasMaxLength(50);
        builder.HasIndex(f => f.Code).IsUnique();

        builder.HasData(BusinessCatalog.FarmingMethodNames
            .Select(entry => new FarmingMethodLookup
            {
                Id = (int)entry.Key,
                Name = entry.Value,
                Code = entry.Key.ToString()
            }));
    }
}

public class CompanyFieldLookupConfiguration : IEntityTypeConfiguration<CompanyFieldLookup>
{
    public void Configure(EntityTypeBuilder<CompanyFieldLookup> builder)
    {
        builder.ToTable("CompanyFields");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();
        builder.Property(c => c.Name).IsRequired().HasMaxLength(150);
        builder.Property(c => c.Code).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Group).IsRequired().HasMaxLength(100);
        builder.Property(c => c.GroupAr).IsRequired().HasMaxLength(100);
        builder.HasIndex(c => c.Code).IsUnique();

        builder.HasData(BusinessCatalog.CompanyFields
            .Select(entry => new CompanyFieldLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                Code = entry.Value.ToString(),
                Group = entry.Group,
                GroupAr = entry.GroupAr
            }));
    }
}

public class SupplierTypeLookupConfiguration : IEntityTypeConfiguration<SupplierTypeLookup>
{
    public void Configure(EntityTypeBuilder<SupplierTypeLookup> builder)
    {
        builder.ToTable("SupplierTypes");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedNever();
        builder.Property(s => s.Name).IsRequired().HasMaxLength(150);
        builder.Property(s => s.Group).IsRequired().HasMaxLength(100);
        builder.Property(s => s.GroupAr).IsRequired().HasMaxLength(100);

        builder.HasIndex(s => new { s.Group, s.Name }).IsUnique();

        builder.HasData(BusinessCatalog.SupplierTypes
            .Select(entry => new SupplierTypeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                Group = entry.Group,
                GroupAr = entry.GroupAr
            }));
    }
}

public class TradeTypeLookupConfiguration : IEntityTypeConfiguration<TradeTypeLookup>
{
    public void Configure(EntityTypeBuilder<TradeTypeLookup> builder)
    {
        builder.ToTable("TradeTypes");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();
        builder.Property(t => t.Name).IsRequired().HasMaxLength(150);
        builder.Property(t => t.Group).IsRequired().HasMaxLength(100);
        builder.Property(t => t.GroupAr).IsRequired().HasMaxLength(100);

        builder.HasIndex(t => new { t.Group, t.Name }).IsUnique();

        builder.HasData(BusinessCatalog.TradeTypes
            .Select(entry => new TradeTypeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                Group = entry.Group,
                GroupAr = entry.GroupAr
            }));
    }
}

public class SaleTypeLookupConfiguration : IEntityTypeConfiguration<SaleTypeLookup>
{
    public void Configure(EntityTypeBuilder<SaleTypeLookup> builder)
    {
        builder.ToTable("SaleTypes");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedNever();
        builder.Property(s => s.Name).IsRequired().HasMaxLength(150);
        builder.Property(s => s.Code).IsRequired().HasMaxLength(50);
        builder.HasIndex(s => s.Code).IsUnique();

        builder.HasData(BusinessCatalog.SaleTypeNames
            .Select(entry => new SaleTypeLookup
            {
                Id = (int)entry.Key,
                Name = entry.Value,
                Code = entry.Key.ToString()
            }));
    }
}
