using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class CamelConfiguration : IEntityTypeConfiguration<Camel>
{
    public void Configure(EntityTypeBuilder<Camel> builder)
    {
        builder.ToTable("Camels");

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
            .WithOne(i => i.Camel)
            .HasForeignKey(i => i.CamelId)
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

public class CamelImageConfiguration : IEntityTypeConfiguration<CamelImage>
{
    public void Configure(EntityTypeBuilder<CamelImage> builder)
    {
        builder.ToTable("CamelImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.CamelId);

        builder.HasQueryFilter(i => !i.Camel.IsDeleted);
    }
}

public class CamelBreedLookupConfiguration : IEntityTypeConfiguration<CamelBreedLookup>
{
    public void Configure(EntityTypeBuilder<CamelBreedLookup> builder)
    {
        builder.ToTable("CamelBreeds");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(CamelCatalog.Breeds
            .Select(entry => new CamelBreedLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class CamelPurposeLookupConfiguration : IEntityTypeConfiguration<CamelPurposeLookup>
{
    public void Configure(EntityTypeBuilder<CamelPurposeLookup> builder)
    {
        builder.ToTable("CamelPurposes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(CamelCatalog.Purposes
            .Select(entry => new CamelPurposeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class CamelAgeLookupConfiguration : IEntityTypeConfiguration<CamelAgeLookup>
{
    public void Configure(EntityTypeBuilder<CamelAgeLookup> builder)
    {
        builder.ToTable("CamelAges");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(CamelCatalog.Ages
            .Select(entry => new CamelAgeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class CamelGenderLookupConfiguration : IEntityTypeConfiguration<CamelGenderLookup>
{
    public void Configure(EntityTypeBuilder<CamelGenderLookup> builder)
    {
        builder.ToTable("CamelGenders");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(CamelCatalog.Genders
            .Select(entry => new CamelGenderLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class CamelHealthStatusLookupConfiguration : IEntityTypeConfiguration<CamelHealthStatusLookup>
{
    public void Configure(EntityTypeBuilder<CamelHealthStatusLookup> builder)
    {
        builder.ToTable("CamelHealthStatuses");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(CamelCatalog.HealthStatuses
            .Select(entry => new CamelHealthStatusLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class CamelVaccinationLookupConfiguration : IEntityTypeConfiguration<CamelVaccinationLookup>
{
    public void Configure(EntityTypeBuilder<CamelVaccinationLookup> builder)
    {
        builder.ToTable("CamelVaccinations");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(CamelCatalog.Vaccinations
            .Select(entry => new CamelVaccinationLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
