using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class BeeConfiguration : IEntityTypeConfiguration<Bee>
{
    public void Configure(EntityTypeBuilder<Bee> builder)
    {
        builder.ToTable("Bees");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SellerName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.OtherType).HasMaxLength(150);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(300);
        builder.Property(x => x.GoogleMaps).HasMaxLength(1000);
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(20);
        builder.Property(x => x.WhatsApp).HasMaxLength(20);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(4000);
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.AnimalType).HasConversion<int>();
        builder.Property(x => x.Purpose).HasConversion<int>();
        builder.Property(x => x.HealthStatus).HasConversion<int>();
        builder.Property(x => x.Production).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.Bee)
            .HasForeignKey(i => i.BeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.SellerName);
        builder.HasIndex(x => x.AnimalType);
        builder.HasIndex(x => x.Purpose);
        builder.HasIndex(x => x.HealthStatus);
        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.UserId);
    }
}

public class BeeImageConfiguration : IEntityTypeConfiguration<BeeImage>
{
    public void Configure(EntityTypeBuilder<BeeImage> builder)
    {
        builder.ToTable("BeeImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.BeeId);

        builder.HasQueryFilter(i => !i.Bee.IsDeleted);
    }
}

public class BeeTypeLookupConfiguration : IEntityTypeConfiguration<BeeTypeLookup>
{
    public void Configure(EntityTypeBuilder<BeeTypeLookup> builder)
    {
        builder.ToTable("BeeTypes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(BeeCatalog.Types
            .Select(entry => new BeeTypeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class BeePurposeLookupConfiguration : IEntityTypeConfiguration<BeePurposeLookup>
{
    public void Configure(EntityTypeBuilder<BeePurposeLookup> builder)
    {
        builder.ToTable("BeePurposes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(BeeCatalog.Purposes
            .Select(entry => new BeePurposeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class BeeHealthStatusLookupConfiguration : IEntityTypeConfiguration<BeeHealthStatusLookup>
{
    public void Configure(EntityTypeBuilder<BeeHealthStatusLookup> builder)
    {
        builder.ToTable("BeeHealthStatuses");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(BeeCatalog.HealthStatuses
            .Select(entry => new BeeHealthStatusLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class BeeProductionLookupConfiguration : IEntityTypeConfiguration<BeeProductionLookup>
{
    public void Configure(EntityTypeBuilder<BeeProductionLookup> builder)
    {
        builder.ToTable("BeeProductions");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(BeeCatalog.Productions
            .Select(entry => new BeeProductionLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
