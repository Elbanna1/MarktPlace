using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("Pets");

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
            .WithOne(i => i.Pet)
            .HasForeignKey(i => i.PetId)
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

public class PetImageConfiguration : IEntityTypeConfiguration<PetImage>
{
    public void Configure(EntityTypeBuilder<PetImage> builder)
    {
        builder.ToTable("PetImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.PetId);

        builder.HasQueryFilter(i => !i.Pet.IsDeleted);
    }
}

public class PetBreedLookupConfiguration : IEntityTypeConfiguration<PetBreedLookup>
{
    public void Configure(EntityTypeBuilder<PetBreedLookup> builder)
    {
        builder.ToTable("PetBreeds");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(PetCatalog.Breeds
            .Select(entry => new PetBreedLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class PetPurposeLookupConfiguration : IEntityTypeConfiguration<PetPurposeLookup>
{
    public void Configure(EntityTypeBuilder<PetPurposeLookup> builder)
    {
        builder.ToTable("PetPurposes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(PetCatalog.Purposes
            .Select(entry => new PetPurposeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class PetAgeLookupConfiguration : IEntityTypeConfiguration<PetAgeLookup>
{
    public void Configure(EntityTypeBuilder<PetAgeLookup> builder)
    {
        builder.ToTable("PetAges");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(PetCatalog.Ages
            .Select(entry => new PetAgeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class PetGenderLookupConfiguration : IEntityTypeConfiguration<PetGenderLookup>
{
    public void Configure(EntityTypeBuilder<PetGenderLookup> builder)
    {
        builder.ToTable("PetGenders");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(PetCatalog.Genders
            .Select(entry => new PetGenderLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class PetHealthStatusLookupConfiguration : IEntityTypeConfiguration<PetHealthStatusLookup>
{
    public void Configure(EntityTypeBuilder<PetHealthStatusLookup> builder)
    {
        builder.ToTable("PetHealthStatuses");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(PetCatalog.HealthStatuses
            .Select(entry => new PetHealthStatusLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class PetTrainingLevelLookupConfiguration : IEntityTypeConfiguration<PetTrainingLevelLookup>
{
    public void Configure(EntityTypeBuilder<PetTrainingLevelLookup> builder)
    {
        builder.ToTable("PetTrainingLevels");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(PetCatalog.TrainingLevels
            .Select(entry => new PetTrainingLevelLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class PetVaccinationLookupConfiguration : IEntityTypeConfiguration<PetVaccinationLookup>
{
    public void Configure(EntityTypeBuilder<PetVaccinationLookup> builder)
    {
        builder.ToTable("PetVaccinations");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(PetCatalog.Vaccinations
            .Select(entry => new PetVaccinationLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
