using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Enums;

namespace Persistence.Configurations;

public class HomeSectionConfiguration : IEntityTypeConfiguration<HomeSection>
{
    private const int HeroId = 1;
    private const int Slider1Id = 2;
    private const int CategoryStripId = 3;
    private const int Slider2Id = 4;
    private const int RecentlyViewedId = 5;

    private const int CategorySectionIdOffset = 100;

    private static readonly DateTime SeedTimestamp = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public void Configure(EntityTypeBuilder<HomeSection> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedNever();

        builder.Property(s => s.Key).IsRequired().HasMaxLength(60);
        builder.HasIndex(s => s.Key).IsUnique();

        builder.Property(s => s.Type).IsRequired();

        builder.Property(s => s.Title).HasMaxLength(150);
        builder.Property(s => s.TitleEn).HasMaxLength(150);
        builder.Property(s => s.Subtitle).HasMaxLength(500);

        builder.Property(s => s.IsVisible).IsRequired().HasDefaultValue(true);
        builder.Property(s => s.SortOrder).IsRequired().HasDefaultValue(0);

        builder.Property(s => s.ImageUrl).HasMaxLength(1000);
        builder.Property(s => s.ImagePath).HasMaxLength(500);
        builder.Property(s => s.LinkUrl).HasMaxLength(1000);
        builder.Property(s => s.LinkText).HasMaxLength(100);

        builder.HasOne(s => s.Category)
            .WithMany()
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(s => s.SortOrder);

        builder.HasData(BuildSeed());
    }

    private static HomeSection[] BuildSeed()
    {
        var fixedSections = new List<HomeSection>
        {
            new()
            {
                Id = HeroId,
                Key = "hero",
                Type = HomeSectionType.Hero,
                Title = "شبيك لبيك",
                TitleEn = "Shobik Lobik",
                Subtitle = "سوق الفيوم — كل ما تحتاجه في مكان واحد",
                IsVisible = true,
                SortOrder = 1,
                CreatedAt = SeedTimestamp
            },
            new()
            {
                Id = Slider1Id,
                Key = "slider1",
                Type = HomeSectionType.Slider1,
                Title = "إعلانات مميزة",
                TitleEn = "Featured",
                IsVisible = true,
                SortOrder = 2,
                CreatedAt = SeedTimestamp
            },
            new()
            {
                Id = CategoryStripId,
                Key = "category-strip",
                Type = HomeSectionType.CategoryStrip,
                Title = "الأقسام",
                TitleEn = "Categories",
                IsVisible = true,
                SortOrder = 3,
                CreatedAt = SeedTimestamp
            },
            new()
            {
                Id = Slider2Id,
                Key = "slider2",
                Type = HomeSectionType.Slider2,
                Title = "إعلانات",
                TitleEn = "Advertisements",
                IsVisible = true,
                SortOrder = 4,
                CreatedAt = SeedTimestamp
            },
            new()
            {
                Id = RecentlyViewedId,
                Key = "recently-viewed",
                Type = HomeSectionType.RecentlyViewed,
                Title = "شوهد مؤخرًا",
                TitleEn = "Recently Viewed",
                IsVisible = true,
                SortOrder = 5,
                CreatedAt = SeedTimestamp
            }
        };

        var categorySections = HomeCategorySeed
            .Select((entry, index) => new HomeSection
            {
                Id = CategorySectionIdOffset + (int)entry.Category,
                Key = entry.Key,
                Type = HomeSectionType.CategorySection,
                Title = entry.Title,
                TitleEn = entry.TitleEn,
                CategoryId = (int)entry.Category,
                ItemCount = 8,
                IsVisible = true,
                SortOrder = 10 + index,
                CreatedAt = SeedTimestamp
            });

        return fixedSections.Concat(categorySections).ToArray();
    }

    private static readonly (CategoryType Category, string Key, string Title, string TitleEn)[] HomeCategorySeed =
    [
        (CategoryType.Cars, "category-cars", "أحدث السيارات", "Latest Cars"),
        (CategoryType.RealEstate, "category-real-estate", "أحدث العقارات", "Latest Real Estate"),
        (CategoryType.Jobs, "category-jobs", "فرص العمل", "Jobs"),
        (CategoryType.Animals, "category-animals", "الحيوانات", "Animals"),
        (CategoryType.Business, "category-business", "رجال أعمال", "Business"),
        (CategoryType.WorkshopsAndCraftsmen, "category-workshops", "الورش والحرفيين", "Workshops & Craftsmen"),
        (CategoryType.Clothing, "category-clothing", "الملابس", "Clothing"),
        (CategoryType.OnlineShopping, "category-online-shopping", "التسوق أونلاين", "Online Shopping"),
        (CategoryType.HomeFurnishing, "category-home-furnishing", "افرش بيتك", "Home Furnishing"),
        (CategoryType.Antiques, "category-antiques", "التحف والأنتيكات", "Antiques"),
        (CategoryType.LostAndFound, "category-lost-found", "المفقودات", "Lost & Found")
    ];
}
