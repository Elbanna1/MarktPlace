using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class HorseConfiguration : IEntityTypeConfiguration<Horse>
{
    public void Configure(EntityTypeBuilder<Horse> builder)
    {
        builder.ToTable("Horses");

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
        builder.Property(x => x.TrainingLevel).HasConversion<int>();
        builder.Property(x => x.Vaccination).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.Horse)
            .HasForeignKey(i => i.HorseId)
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

public class HorseImageConfiguration : IEntityTypeConfiguration<HorseImage>
{
    public void Configure(EntityTypeBuilder<HorseImage> builder)
    {
        builder.ToTable("HorseImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.HorseId);

        builder.HasQueryFilter(i => !i.Horse.IsDeleted);
    }
}

public class HorseBreedLookupConfiguration : IEntityTypeConfiguration<HorseBreedLookup>
{
    public void Configure(EntityTypeBuilder<HorseBreedLookup> builder)
    {
        builder.ToTable("HorseBreeds");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(HorseCatalog.Breeds
            .Select(entry => new HorseBreedLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class HorsePurposeLookupConfiguration : IEntityTypeConfiguration<HorsePurposeLookup>
{
    public void Configure(EntityTypeBuilder<HorsePurposeLookup> builder)
    {
        builder.ToTable("HorsePurposes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(HorseCatalog.Purposes
            .Select(entry => new HorsePurposeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class HorseAgeLookupConfiguration : IEntityTypeConfiguration<HorseAgeLookup>
{
    public void Configure(EntityTypeBuilder<HorseAgeLookup> builder)
    {
        builder.ToTable("HorseAges");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(HorseCatalog.Ages
            .Select(entry => new HorseAgeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class HorseGenderLookupConfiguration : IEntityTypeConfiguration<HorseGenderLookup>
{
    public void Configure(EntityTypeBuilder<HorseGenderLookup> builder)
    {
        builder.ToTable("HorseGenders");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(HorseCatalog.Genders
            .Select(entry => new HorseGenderLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class HorseHealthStatusLookupConfiguration : IEntityTypeConfiguration<HorseHealthStatusLookup>
{
    public void Configure(EntityTypeBuilder<HorseHealthStatusLookup> builder)
    {
        builder.ToTable("HorseHealthStatuses");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(HorseCatalog.HealthStatuses
            .Select(entry => new HorseHealthStatusLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class HorseTrainingLevelLookupConfiguration : IEntityTypeConfiguration<HorseTrainingLevelLookup>
{
    public void Configure(EntityTypeBuilder<HorseTrainingLevelLookup> builder)
    {
        builder.ToTable("HorseTrainingLevels");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(HorseCatalog.TrainingLevels
            .Select(entry => new HorseTrainingLevelLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class HorseVaccinationLookupConfiguration : IEntityTypeConfiguration<HorseVaccinationLookup>
{
    public void Configure(EntityTypeBuilder<HorseVaccinationLookup> builder)
    {
        builder.ToTable("HorseVaccinations");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(HorseCatalog.Vaccinations
            .Select(entry => new HorseVaccinationLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
