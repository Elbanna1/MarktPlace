using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class HandmadeConfiguration : IEntityTypeConfiguration<Handmade>
{
    public void Configure(EntityTypeBuilder<Handmade> builder)
    {
        builder.ToTable("Handmades");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SellerName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(20);
        builder.Property(x => x.WhatsApp).HasMaxLength(20);
        builder.Property(x => x.ProductName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.OtherType).HasMaxLength(150);
        builder.Property(x => x.Material).IsRequired().HasMaxLength(250);
        builder.Property(x => x.ProductionTime).HasMaxLength(100);
        builder.Property(x => x.Size).HasMaxLength(100);
        builder.Property(x => x.OtherColor).HasMaxLength(150);
        builder.Property(x => x.Governorate).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Center).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(300);
        builder.Property(x => x.GoogleMaps).HasMaxLength(1000);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(4000);
        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

        builder.Property(x => x.HandmadeType).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Colors)
            .WithOne(c => c.Handmade)
            .HasForeignKey(c => c.HandmadeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.Handmade)
            .HasForeignKey(i => i.HandmadeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Video)
            .WithOne(v => v.Handmade)
            .HasForeignKey<HandmadeVideo>(v => v.HandmadeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.SellerName);
        builder.HasIndex(x => x.HandmadeType);
        builder.HasIndex(x => x.IsFullyHandmade);
        builder.HasIndex(x => x.CustomOrder);
        builder.HasIndex(x => x.Negotiable);
        builder.HasIndex(x => x.Center);
        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.UserId);
    }
}

public class HandmadeColorSelectionConfiguration : IEntityTypeConfiguration<HandmadeColorSelection>
{
    public void Configure(EntityTypeBuilder<HandmadeColorSelection> builder)
    {
        builder.ToTable("HandmadeColorSelections");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Color).HasConversion<int>();

        builder.HasIndex(c => new { c.HandmadeId, c.Color }).IsUnique();
        builder.HasIndex(c => c.Color);

        builder.HasQueryFilter(c => !c.Handmade.IsDeleted);
    }
}

public class HandmadeImageConfiguration : IEntityTypeConfiguration<HandmadeImage>
{
    public void Configure(EntityTypeBuilder<HandmadeImage> builder)
    {
        builder.ToTable("HandmadeImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(i => i.HandmadeId);

        builder.HasQueryFilter(i => !i.Handmade.IsDeleted);
    }
}

public class HandmadeVideoConfiguration : IEntityTypeConfiguration<HandmadeVideo>
{
    public void Configure(EntityTypeBuilder<HandmadeVideo> builder)
    {
        builder.ToTable("HandmadeVideos");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.FileName).IsRequired().HasMaxLength(256);
        builder.Property(v => v.VideoPath).IsRequired().HasMaxLength(512);
        builder.Property(v => v.VideoUrl).IsRequired().HasMaxLength(1024);

        builder.HasIndex(v => v.HandmadeId).IsUnique();

        builder.HasQueryFilter(v => !v.Handmade.IsDeleted);
    }
}

public class HandmadeTypeLookupConfiguration : IEntityTypeConfiguration<HandmadeTypeLookup>
{
    public void Configure(EntityTypeBuilder<HandmadeTypeLookup> builder)
    {
        builder.ToTable("HandmadeTypes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(HandmadeCatalog.Types
            .Select(entry => new HandmadeTypeLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}

public class HandmadeColorLookupConfiguration : IEntityTypeConfiguration<HandmadeColorLookup>
{
    public void Configure(EntityTypeBuilder<HandmadeColorLookup> builder)
    {
        builder.ToTable("HandmadeColors");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(HandmadeCatalog.Colors
            .Select(entry => new HandmadeColorLookup
            {
                Id = (int)entry.Value,
                Name = entry.Name,
                NameEn = entry.NameEn
            }));
    }
}
