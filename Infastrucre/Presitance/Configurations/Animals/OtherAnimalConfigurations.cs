using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class OtherAnimalConfiguration : IEntityTypeConfiguration<OtherAnimal>
{
    public void Configure(EntityTypeBuilder<OtherAnimal> builder)
    {
        builder.ToTable("OtherAnimals");

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
            .WithOne(i => i.OtherAnimal)
            .HasForeignKey(i => i.OtherAnimalId)
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

public class OtherAnimalImageConfiguration : IEntityTypeConfiguration<OtherAnimalImage>
{
    public void Configure(EntityTypeBuilder<OtherAnimalImage> builder)
    {
        builder.ToTable("OtherAnimalImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.OtherAnimalId);

        builder.HasQueryFilter(i => !i.OtherAnimal.IsDeleted);
    }
}

public class OtherAnimalTypeLookupConfiguration : IEntityTypeConfiguration<OtherAnimalTypeLookup>
{
    public void Configure(EntityTypeBuilder<OtherAnimalTypeLookup> builder)
    {
        builder.ToTable("OtherAnimalTypes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(OtherAnimalCatalog.Types
            .Select(entry => new OtherAnimalTypeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class OtherAnimalPurposeLookupConfiguration : IEntityTypeConfiguration<OtherAnimalPurposeLookup>
{
    public void Configure(EntityTypeBuilder<OtherAnimalPurposeLookup> builder)
    {
        builder.ToTable("OtherAnimalPurposes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(OtherAnimalCatalog.Purposes
            .Select(entry => new OtherAnimalPurposeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class OtherAnimalAgeLookupConfiguration : IEntityTypeConfiguration<OtherAnimalAgeLookup>
{
    public void Configure(EntityTypeBuilder<OtherAnimalAgeLookup> builder)
    {
        builder.ToTable("OtherAnimalAges");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(OtherAnimalCatalog.Ages
            .Select(entry => new OtherAnimalAgeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class OtherAnimalGenderLookupConfiguration : IEntityTypeConfiguration<OtherAnimalGenderLookup>
{
    public void Configure(EntityTypeBuilder<OtherAnimalGenderLookup> builder)
    {
        builder.ToTable("OtherAnimalGenders");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(OtherAnimalCatalog.Genders
            .Select(entry => new OtherAnimalGenderLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class OtherAnimalHealthStatusLookupConfiguration : IEntityTypeConfiguration<OtherAnimalHealthStatusLookup>
{
    public void Configure(EntityTypeBuilder<OtherAnimalHealthStatusLookup> builder)
    {
        builder.ToTable("OtherAnimalHealthStatuses");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(OtherAnimalCatalog.HealthStatuses
            .Select(entry => new OtherAnimalHealthStatusLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class OtherAnimalVaccinationLookupConfiguration : IEntityTypeConfiguration<OtherAnimalVaccinationLookup>
{
    public void Configure(EntityTypeBuilder<OtherAnimalVaccinationLookup> builder)
    {
        builder.ToTable("OtherAnimalVaccinations");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(OtherAnimalCatalog.Vaccinations
            .Select(entry => new OtherAnimalVaccinationLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
