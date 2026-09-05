using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Enums;

namespace Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).ValueGeneratedNever();
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.NameAr).IsRequired().HasMaxLength(100);

        builder.Property(c => c.Icon).HasMaxLength(500);

        builder.Property(c => c.IsActive).IsRequired().HasDefaultValue(true);

        builder.Property(c => c.SortOrder).IsRequired().HasDefaultValue(0);

        builder.HasIndex(c => new { c.SortOrder, c.Id });

        builder.HasData(
            new Category
            {
                Id = (int)CategoryType.Cars,
                Name = "Cars",
                NameAr = "سيارات"
            },
            new Category
            {
                Id = (int)CategoryType.WorkshopsAndCraftsmen,
                Name = "Workshops & Craftsmen",
                NameAr = "الورش والحرفيين"
            },
            new Category
            {
                Id = (int)CategoryType.LostAndFound,
                Name = "Lost & Found",
                NameAr = "المفقودات"
            },
            new Category
            {
                Id = (int)CategoryType.Business,
                Name = "Business",
                NameAr = "رجال أعمال"
            },
            new Category
            {
                Id = (int)CategoryType.Jobs,
                Name = "Jobs",
                NameAr = "الوظائف"
            },
            new Category
            {
                Id = (int)CategoryType.Animals,
                Name = "Animals",
                NameAr = "الحيوانات"
            },
            new Category
            {
                Id = (int)CategoryType.Antiques,
                Name = "Antiques",
                NameAr = "التحف والأنتيكات"
            },
            new Category
            {
                Id = (int)CategoryType.Clothing,
                Name = "Clothing",
                NameAr = "الملابس"
            },
            new Category
            {
                Id = (int)CategoryType.OnlineShopping,
                Name = "Online Shopping",
                NameAr = "التسوق أونلاين"
            },
            new Category
            {
                Id = (int)CategoryType.HomeFurnishing,
                Name = "Home Furnishing",
                NameAr = "افرش بيتك"
            },
            new Category
            {
                Id = (int)CategoryType.RealEstate,
                Name = "Real Estate",
                NameAr = "عقارات"
            },
            new Category
            {
                Id = (int)CategoryType.Charity,
                Name = "Charity",

                NameAr = "بوابة الخيرات"
            });
    }
}
