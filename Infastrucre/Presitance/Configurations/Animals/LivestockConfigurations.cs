using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class LivestockConfiguration : IEntityTypeConfiguration<Livestock>
{
    public void Configure(EntityTypeBuilder<Livestock> builder)
    {
        builder.ToTable("Livestock");

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
        builder.Property(x => x.Production).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.Livestock)
            .HasForeignKey(i => i.LivestockId)
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

public class LivestockImageConfiguration : IEntityTypeConfiguration<LivestockImage>
{
    public void Configure(EntityTypeBuilder<LivestockImage> builder)
    {
        builder.ToTable("LivestockImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.LivestockId);

        builder.HasQueryFilter(i => !i.Livestock.IsDeleted);
    }
}

public class LivestockBreedLookupConfiguration : IEntityTypeConfiguration<LivestockBreedLookup>
{
    public void Configure(EntityTypeBuilder<LivestockBreedLookup> builder)
    {
        builder.ToTable("LivestockBreeds");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(LivestockCatalog.Breeds
            .Select(entry => new LivestockBreedLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class LivestockPurposeLookupConfiguration : IEntityTypeConfiguration<LivestockPurposeLookup>
{
    public void Configure(EntityTypeBuilder<LivestockPurposeLookup> builder)
    {
        builder.ToTable("LivestockPurposes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(LivestockCatalog.Purposes
            .Select(entry => new LivestockPurposeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class LivestockAgeLookupConfiguration : IEntityTypeConfiguration<LivestockAgeLookup>
{
    public void Configure(EntityTypeBuilder<LivestockAgeLookup> builder)
    {
        builder.ToTable("LivestockAges");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(LivestockCatalog.Ages
            .Select(entry => new LivestockAgeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class LivestockGenderLookupConfiguration : IEntityTypeConfiguration<LivestockGenderLookup>
{
    public void Configure(EntityTypeBuilder<LivestockGenderLookup> builder)
    {
        builder.ToTable("LivestockGenders");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(LivestockCatalog.Genders
            .Select(entry => new LivestockGenderLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class LivestockHealthStatusLookupConfiguration : IEntityTypeConfiguration<LivestockHealthStatusLookup>
{
    public void Configure(EntityTypeBuilder<LivestockHealthStatusLookup> builder)
    {
        builder.ToTable("LivestockHealthStatuses");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(LivestockCatalog.HealthStatuses
            .Select(entry => new LivestockHealthStatusLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class LivestockVaccinationLookupConfiguration : IEntityTypeConfiguration<LivestockVaccinationLookup>
{
    public void Configure(EntityTypeBuilder<LivestockVaccinationLookup> builder)
    {
        builder.ToTable("LivestockVaccinations");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(LivestockCatalog.Vaccinations
            .Select(entry => new LivestockVaccinationLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class LivestockProductionLookupConfiguration : IEntityTypeConfiguration<LivestockProductionLookup>
{
    public void Configure(EntityTypeBuilder<LivestockProductionLookup> builder)
    {
        builder.ToTable("LivestockProductions");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(LivestockCatalog.Productions
            .Select(entry => new LivestockProductionLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
