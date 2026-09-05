using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class HomemadeFoodConfiguration : IEntityTypeConfiguration<HomemadeFood>
{
    public void Configure(EntityTypeBuilder<HomemadeFood> builder)
    {
        builder.ToTable("HomemadeFoods");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProjectName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(20);
        builder.Property(x => x.WhatsApp).IsRequired().HasMaxLength(20);
        builder.Property(x => x.OtherSection).HasMaxLength(150);
        builder.Property(x => x.PreparationTime).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Ingredients).HasMaxLength(2000);
        builder.Property(x => x.WeightOrSize).HasMaxLength(150);
        builder.Property(x => x.StorageMethod).HasMaxLength(500);
        builder.Property(x => x.AvailableOrderingHours).HasMaxLength(200);
        builder.Property(x => x.AdditionalNotes).HasMaxLength(2000);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(4000);
        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

        builder.Property(x => x.VideoPath).HasMaxLength(500);
        builder.Property(x => x.VideoUrl).HasMaxLength(1000);

        builder.Property(x => x.Section).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.HomemadeFood)
            .HasForeignKey(i => i.HomemadeFoodId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.DeliveryAreas)
            .WithOne(a => a.HomemadeFood)
            .HasForeignKey(a => a.HomemadeFoodId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.ProjectName);
        builder.HasIndex(x => x.Section);
        builder.HasIndex(x => x.PreparedOnDemand);
        builder.HasIndex(x => x.DeliveryAvailable);
        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.UserId);
    }
}

public class HomemadeFoodImageConfiguration : IEntityTypeConfiguration<HomemadeFoodImage>
{
    public void Configure(EntityTypeBuilder<HomemadeFoodImage> builder)
    {
        builder.ToTable("HomemadeFoodImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.HomemadeFoodId);

        builder.HasQueryFilter(i => !i.HomemadeFood.IsDeleted);
    }
}

public class HomemadeFoodDeliveryAreaSelectionConfiguration
    : IEntityTypeConfiguration<HomemadeFoodDeliveryAreaSelection>
{
    public void Configure(EntityTypeBuilder<HomemadeFoodDeliveryAreaSelection> builder)
    {
        builder.ToTable("HomemadeFoodDeliveryAreaSelections");

        builder.HasKey(x => new { x.HomemadeFoodId, x.DeliveryArea });
        builder.Property(x => x.DeliveryArea).HasConversion<int>();

        builder.HasIndex(x => x.DeliveryArea);

        builder.HasQueryFilter(x => !x.HomemadeFood.IsDeleted);
    }
}

public class HomemadeFoodSectionLookupConfiguration : IEntityTypeConfiguration<HomemadeFoodSectionLookup>
{
    public void Configure(EntityTypeBuilder<HomemadeFoodSectionLookup> builder)
    {
        builder.ToTable("HomemadeFoodSections");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(HomemadeFoodCatalog.Sections
            .Select(entry => new HomemadeFoodSectionLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class HomemadeFoodDeliveryAreaLookupConfiguration
    : IEntityTypeConfiguration<HomemadeFoodDeliveryAreaLookup>
{
    public void Configure(EntityTypeBuilder<HomemadeFoodDeliveryAreaLookup> builder)
    {
        builder.ToTable("HomemadeFoodDeliveryAreas");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(HomemadeFoodCatalog.DeliveryAreas
            .Select(entry => new HomemadeFoodDeliveryAreaLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
