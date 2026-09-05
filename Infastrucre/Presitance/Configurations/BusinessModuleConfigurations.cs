using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class FactoryConfiguration : IEntityTypeConfiguration<Factory>
{
    public void Configure(EntityTypeBuilder<Factory> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.FactoryName).IsRequired().HasMaxLength(150);
        builder.Property(f => f.OtherSpecialty).HasMaxLength(150);
        builder.Property(f => f.Address).IsRequired().HasMaxLength(300);
        builder.Property(f => f.GoogleMaps).HasMaxLength(1000);
        builder.Property(f => f.Phone).IsRequired().HasMaxLength(20);
        builder.Property(f => f.WhatsApp).IsRequired().HasMaxLength(20);
        builder.Property(f => f.Email).HasMaxLength(256);
        builder.Property(f => f.Title).IsRequired().HasMaxLength(150);
        builder.Property(f => f.Description).IsRequired().HasMaxLength(4000);
        builder.Property(f => f.UserId).IsRequired();

        builder.Property(f => f.ProductionSpecialty).HasConversion<int>();

        builder.HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(f => f.Images)
            .WithOne(i => i.Factory)
            .HasForeignKey(i => i.FactoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, f => !f.IsDeleted);

        builder.HasIndex(f => f.CreatedAt);
        builder.HasIndex(f => f.ProductionSpecialty);
        builder.HasIndex(f => f.FactoryName);
        builder.HasIndex(f => f.UserId);
    }
}

public class FactoryImageConfiguration : IEntityTypeConfiguration<FactoryImage>
{
    public void Configure(EntityTypeBuilder<FactoryImage> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.FactoryId);

        builder.HasQueryFilter(i => !i.Factory.IsDeleted);
    }
}

public class FarmConfiguration : IEntityTypeConfiguration<Farm>
{
    public void Configure(EntityTypeBuilder<Farm> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.FarmName).IsRequired().HasMaxLength(150);
        builder.Property(f => f.OtherFarmType).HasMaxLength(150);
        builder.Property(f => f.Address).IsRequired().HasMaxLength(300);
        builder.Property(f => f.GoogleMaps).HasMaxLength(1000);
        builder.Property(f => f.Phone).IsRequired().HasMaxLength(20);
        builder.Property(f => f.WhatsApp).IsRequired().HasMaxLength(20);
        builder.Property(f => f.Email).HasMaxLength(256);
        builder.Property(f => f.AvailableQuantity).HasMaxLength(150);
        builder.Property(f => f.Title).IsRequired().HasMaxLength(150);
        builder.Property(f => f.Description).IsRequired().HasMaxLength(4000);
        builder.Property(f => f.UserId).IsRequired();

        builder.Property(f => f.AreaInFeddan).HasColumnType("decimal(18,2)");

        builder.Property(f => f.FarmType).HasConversion<int>();
        builder.Property(f => f.AvailabilitySeason).HasConversion<int>();
        builder.Property(f => f.FarmingMethod).HasConversion<int>();

        builder.HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(f => f.Images)
            .WithOne(i => i.Farm)
            .HasForeignKey(i => i.FarmId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, f => !f.IsDeleted);

        builder.HasIndex(f => f.CreatedAt);
        builder.HasIndex(f => f.FarmType);
        builder.HasIndex(f => f.FarmName);
        builder.HasIndex(f => f.UserId);
    }
}

public class FarmImageConfiguration : IEntityTypeConfiguration<FarmImage>
{
    public void Configure(EntityTypeBuilder<FarmImage> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.FarmId);

        builder.HasQueryFilter(i => !i.Farm.IsDeleted);
    }
}

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.CompanyName).IsRequired().HasMaxLength(150);
        builder.Property(c => c.OtherCompanyField).HasMaxLength(150);
        builder.Property(c => c.Address).IsRequired().HasMaxLength(300);
        builder.Property(c => c.GoogleMaps).HasMaxLength(1000);
        builder.Property(c => c.Phone).IsRequired().HasMaxLength(20);
        builder.Property(c => c.WhatsApp).IsRequired().HasMaxLength(20);
        builder.Property(c => c.Email).HasMaxLength(256);
        builder.Property(c => c.Website).HasMaxLength(1000);
        builder.Property(c => c.LogoPath).HasMaxLength(512);
        builder.Property(c => c.LogoUrl).HasMaxLength(1024);
        builder.Property(c => c.Title).IsRequired().HasMaxLength(150);
        builder.Property(c => c.Description).IsRequired().HasMaxLength(4000);
        builder.Property(c => c.UserId).IsRequired();

        builder.Property(c => c.CompanyField).HasConversion<int>();

        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(c => c.Images)
            .WithOne(i => i.Company)
            .HasForeignKey(i => i.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, c => !c.IsDeleted);

        builder.HasIndex(c => c.CreatedAt);
        builder.HasIndex(c => c.CompanyField);
        builder.HasIndex(c => c.CompanyName);
        builder.HasIndex(c => c.UserId);
    }
}

public class CompanyImageConfiguration : IEntityTypeConfiguration<CompanyImage>
{
    public void Configure(EntityTypeBuilder<CompanyImage> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.CompanyId);

        builder.HasQueryFilter(i => !i.Company.IsDeleted);
    }
}
