using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class SheepGoatConfiguration : IEntityTypeConfiguration<SheepGoat>
{
    public void Configure(EntityTypeBuilder<SheepGoat> builder)
    {
        builder.ToTable("SheepGoats");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SellerName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.OtherBreed).HasMaxLength(150);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(300);
        builder.Property(x => x.GoogleMaps).HasMaxLength(1000);
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(20);
        builder.Property(x => x.WhatsApp).HasMaxLength(20);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(4000);
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.Breed).HasConversion<int>();
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
            .WithOne(i => i.SheepGoat)
            .HasForeignKey(i => i.SheepGoatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.SellerName);
        builder.HasIndex(x => x.Breed);
        builder.HasIndex(x => x.Purpose);
        builder.HasIndex(x => x.Age);
        builder.HasIndex(x => x.Gender);
        builder.HasIndex(x => x.HealthStatus);
        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.UserId);
    }
}

public class SheepGoatImageConfiguration : IEntityTypeConfiguration<SheepGoatImage>
{
    public void Configure(EntityTypeBuilder<SheepGoatImage> builder)
    {
        builder.ToTable("SheepGoatImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.SheepGoatId);

        builder.HasQueryFilter(i => !i.SheepGoat.IsDeleted);
    }
}

public class SheepGoatBreedLookupConfiguration : IEntityTypeConfiguration<SheepGoatBreedLookup>
{
    public void Configure(EntityTypeBuilder<SheepGoatBreedLookup> builder)
    {
        builder.ToTable("SheepGoatBreeds");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(SheepGoatCatalog.Breeds
            .Select(entry => new SheepGoatBreedLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class SheepGoatPurposeLookupConfiguration : IEntityTypeConfiguration<SheepGoatPurposeLookup>
{
    public void Configure(EntityTypeBuilder<SheepGoatPurposeLookup> builder)
    {
        builder.ToTable("SheepGoatPurposes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(SheepGoatCatalog.Purposes
            .Select(entry => new SheepGoatPurposeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class SheepGoatAgeLookupConfiguration : IEntityTypeConfiguration<SheepGoatAgeLookup>
{
    public void Configure(EntityTypeBuilder<SheepGoatAgeLookup> builder)
    {
        builder.ToTable("SheepGoatAges");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(SheepGoatCatalog.Ages
            .Select(entry => new SheepGoatAgeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class SheepGoatGenderLookupConfiguration : IEntityTypeConfiguration<SheepGoatGenderLookup>
{
    public void Configure(EntityTypeBuilder<SheepGoatGenderLookup> builder)
    {
        builder.ToTable("SheepGoatGenders");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(SheepGoatCatalog.Genders
            .Select(entry => new SheepGoatGenderLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class SheepGoatHealthStatusLookupConfiguration : IEntityTypeConfiguration<SheepGoatHealthStatusLookup>
{
    public void Configure(EntityTypeBuilder<SheepGoatHealthStatusLookup> builder)
    {
        builder.ToTable("SheepGoatHealthStatuses");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(SheepGoatCatalog.HealthStatuses
            .Select(entry => new SheepGoatHealthStatusLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class SheepGoatVaccinationLookupConfiguration : IEntityTypeConfiguration<SheepGoatVaccinationLookup>
{
    public void Configure(EntityTypeBuilder<SheepGoatVaccinationLookup> builder)
    {
        builder.ToTable("SheepGoatVaccinations");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(SheepGoatCatalog.Vaccinations
            .Select(entry => new SheepGoatVaccinationLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
