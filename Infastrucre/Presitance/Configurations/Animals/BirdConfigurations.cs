using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class BirdConfiguration : IEntityTypeConfiguration<Bird>
{
    public void Configure(EntityTypeBuilder<Bird> builder)
    {
        builder.ToTable("Birds");

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
        builder.Property(x => x.Age).HasConversion<int>();
        builder.Property(x => x.Gender).HasConversion<int>();
        builder.Property(x => x.HealthStatus).HasConversion<int>();
        builder.Property(x => x.Vaccination).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.Bird)
            .HasForeignKey(i => i.BirdId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.SellerName);
        builder.HasIndex(x => x.AnimalType);
        builder.HasIndex(x => x.Purpose);
        builder.HasIndex(x => x.Age);
        builder.HasIndex(x => x.Gender);
        builder.HasIndex(x => x.HealthStatus);
        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.UserId);
    }
}

public class BirdImageConfiguration : IEntityTypeConfiguration<BirdImage>
{
    public void Configure(EntityTypeBuilder<BirdImage> builder)
    {
        builder.ToTable("BirdImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.BirdId);

        builder.HasQueryFilter(i => !i.Bird.IsDeleted);
    }
}

public class BirdTypeLookupConfiguration : IEntityTypeConfiguration<BirdTypeLookup>
{
    public void Configure(EntityTypeBuilder<BirdTypeLookup> builder)
    {
        builder.ToTable("BirdTypes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(BirdCatalog.Types
            .Select(entry => new BirdTypeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class BirdPurposeLookupConfiguration : IEntityTypeConfiguration<BirdPurposeLookup>
{
    public void Configure(EntityTypeBuilder<BirdPurposeLookup> builder)
    {
        builder.ToTable("BirdPurposes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(BirdCatalog.Purposes
            .Select(entry => new BirdPurposeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class BirdAgeLookupConfiguration : IEntityTypeConfiguration<BirdAgeLookup>
{
    public void Configure(EntityTypeBuilder<BirdAgeLookup> builder)
    {
        builder.ToTable("BirdAges");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(BirdCatalog.Ages
            .Select(entry => new BirdAgeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class BirdGenderLookupConfiguration : IEntityTypeConfiguration<BirdGenderLookup>
{
    public void Configure(EntityTypeBuilder<BirdGenderLookup> builder)
    {
        builder.ToTable("BirdGenders");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(BirdCatalog.Genders
            .Select(entry => new BirdGenderLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class BirdHealthStatusLookupConfiguration : IEntityTypeConfiguration<BirdHealthStatusLookup>
{
    public void Configure(EntityTypeBuilder<BirdHealthStatusLookup> builder)
    {
        builder.ToTable("BirdHealthStatuses");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(BirdCatalog.HealthStatuses
            .Select(entry => new BirdHealthStatusLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class BirdVaccinationLookupConfiguration : IEntityTypeConfiguration<BirdVaccinationLookup>
{
    public void Configure(EntityTypeBuilder<BirdVaccinationLookup> builder)
    {
        builder.ToTable("BirdVaccinations");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(BirdCatalog.Vaccinations
            .Select(entry => new BirdVaccinationLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
