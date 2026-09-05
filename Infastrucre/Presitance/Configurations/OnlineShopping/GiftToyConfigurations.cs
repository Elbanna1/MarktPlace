using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class GiftToyConfiguration : IEntityTypeConfiguration<GiftToy>
{
    public void Configure(EntityTypeBuilder<GiftToy> builder)
    {
        builder.ToTable("GiftToys");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StoreName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(20);
        builder.Property(x => x.WhatsApp).IsRequired().HasMaxLength(20);
        builder.Property(x => x.OtherGiftType).HasMaxLength(150);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(4000);
        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

        builder.Property(x => x.VideoPath).HasMaxLength(500);
        builder.Property(x => x.VideoUrl).HasMaxLength(1000);

        builder.Property(x => x.GiftType).HasConversion<int>();
        builder.Property(x => x.SuitableFor).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.GiftToy)
            .HasForeignKey(i => i.GiftToyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.StoreName);
        builder.HasIndex(x => x.GiftType);
        builder.HasIndex(x => x.SuitableFor);
        builder.HasIndex(x => x.GiftWrapping);
        builder.HasIndex(x => x.DeliveryAvailable);
        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.UserId);
    }
}

public class GiftToyImageConfiguration : IEntityTypeConfiguration<GiftToyImage>
{
    public void Configure(EntityTypeBuilder<GiftToyImage> builder)
    {
        builder.ToTable("GiftToyImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.GiftToyId);

        builder.HasQueryFilter(i => !i.GiftToy.IsDeleted);
    }
}

public class GiftToyTypeLookupConfiguration : IEntityTypeConfiguration<GiftToyTypeLookup>
{
    public void Configure(EntityTypeBuilder<GiftToyTypeLookup> builder)
    {
        builder.ToTable("GiftToyTypes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(GiftToyCatalog.Types
            .Select(entry => new GiftToyTypeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class GiftToySuitableForLookupConfiguration : IEntityTypeConfiguration<GiftToySuitableForLookup>
{
    public void Configure(EntityTypeBuilder<GiftToySuitableForLookup> builder)
    {
        builder.ToTable("GiftToySuitableFor");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(GiftToyCatalog.SuitableFor
            .Select(entry => new GiftToySuitableForLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
