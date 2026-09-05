using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Enums;

namespace Persistence.Configurations;

public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
{
    public void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).ValueGeneratedNever();
        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.NameAr).IsRequired().HasMaxLength(100);

        builder.Property(s => s.Icon).HasMaxLength(500);
        builder.Property(s => s.IsActive).IsRequired().HasDefaultValue(true);

        builder.Property(s => s.SortOrder).IsRequired().HasDefaultValue(0);

        builder.HasIndex(s => new { s.CategoryId, s.SortOrder, s.Id });

        builder.HasOne(s => s.Category)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new SubCategory { Id = (int)SubCategoryType.Private, CategoryId = (int)CategoryType.Cars, Name = "Private", NameAr = "ملاكي" },
            new SubCategory { Id = (int)SubCategoryType.Taxi, CategoryId = (int)CategoryType.Cars, Name = "Taxi", NameAr = "أجرة" },
            new SubCategory { Id = (int)SubCategoryType.Motorcycles, CategoryId = (int)CategoryType.Cars, Name = "Motorcycles", NameAr = "موتوسيكلات" },
            new SubCategory { Id = (int)SubCategoryType.HeavyEquipment, CategoryId = (int)CategoryType.Cars, Name = "Heavy Equipment", NameAr = "لوادر ومعدات ثقيلة" },
            new SubCategory { Id = (int)SubCategoryType.Workshops, CategoryId = (int)CategoryType.WorkshopsAndCraftsmen, Name = "Workshops", NameAr = "الورش" },
            new SubCategory { Id = (int)SubCategoryType.Craftsmen, CategoryId = (int)CategoryType.WorkshopsAndCraftsmen, Name = "Craftsmen", NameAr = "الحرفيين" },
            new SubCategory { Id = (int)SubCategoryType.LostItems, CategoryId = (int)CategoryType.LostAndFound, Name = "Lost", NameAr = "ضايع مني" },
            new SubCategory { Id = (int)SubCategoryType.FoundItems, CategoryId = (int)CategoryType.LostAndFound, Name = "Found", NameAr = "لقيت" },

            new SubCategory { Id = (int)SubCategoryType.Factories, CategoryId = (int)CategoryType.Business, Name = "Factories", NameAr = "المصانع" },
            new SubCategory { Id = (int)SubCategoryType.Farms, CategoryId = (int)CategoryType.Business, Name = "Farms", NameAr = "المزارع" },
            new SubCategory { Id = (int)SubCategoryType.Companies, CategoryId = (int)CategoryType.Business, Name = "Companies", NameAr = "الشركات" },
            new SubCategory { Id = (int)SubCategoryType.Suppliers, CategoryId = (int)CategoryType.Business, Name = "Suppliers", NameAr = "الموردون" },
            new SubCategory { Id = (int)SubCategoryType.WholesaleTraders, CategoryId = (int)CategoryType.Business, Name = "Wholesale Traders", NameAr = "تجار الجملة" },
            new SubCategory { Id = (int)SubCategoryType.FruitAndVegetableTraders, CategoryId = (int)CategoryType.Business, Name = "Fruit & Vegetable Traders", NameAr = "تجار خضر وفاكهة" },

            new SubCategory { Id = (int)SubCategoryType.JobRequests, CategoryId = (int)CategoryType.Jobs, Name = "Job Requests", NameAr = "طلبات عمل" },
            new SubCategory { Id = (int)SubCategoryType.JobOpportunities, CategoryId = (int)CategoryType.Jobs, Name = "Job Opportunities", NameAr = "فرص عمل" },

            new SubCategory { Id = (int)SubCategoryType.Livestock, CategoryId = (int)CategoryType.Animals, Name = "Livestock", NameAr = "المواشي" },
            new SubCategory { Id = (int)SubCategoryType.SheepAndGoats, CategoryId = (int)CategoryType.Animals, Name = "Sheep & Goats", NameAr = "الأغنام والماعز" },
            new SubCategory { Id = (int)SubCategoryType.Horses, CategoryId = (int)CategoryType.Animals, Name = "Horses", NameAr = "الخيول" },
            new SubCategory { Id = (int)SubCategoryType.Camels, CategoryId = (int)CategoryType.Animals, Name = "Camels", NameAr = "الإبل" },
            new SubCategory { Id = (int)SubCategoryType.Birds, CategoryId = (int)CategoryType.Animals, Name = "Birds", NameAr = "الطيور" },
            new SubCategory { Id = (int)SubCategoryType.Pets, CategoryId = (int)CategoryType.Animals, Name = "Pets", NameAr = "الحيوانات الأليفة" },
            new SubCategory { Id = (int)SubCategoryType.Fish, CategoryId = (int)CategoryType.Animals, Name = "Fish", NameAr = "الأسماك" },
            new SubCategory { Id = (int)SubCategoryType.Bees, CategoryId = (int)CategoryType.Animals, Name = "Bees", NameAr = "النحل" },
            new SubCategory { Id = (int)SubCategoryType.OtherAnimals, CategoryId = (int)CategoryType.Animals, Name = "Other Animals", NameAr = "حيوانات أخرى" },

            new SubCategory { Id = (int)SubCategoryType.DecorAntiques, CategoryId = (int)CategoryType.Antiques, Name = "Antiques Decor", NameAr = "تحف" },
            new SubCategory { Id = (int)SubCategoryType.Antiques, CategoryId = (int)CategoryType.Antiques, Name = "Antiques", NameAr = "أنتيكات" },
            new SubCategory { Id = (int)SubCategoryType.Paintings, CategoryId = (int)CategoryType.Antiques, Name = "Paintings", NameAr = "لوحات فنية" },
            new SubCategory { Id = (int)SubCategoryType.Handmade, CategoryId = (int)CategoryType.Antiques, Name = "Handmade", NameAr = "أعمال يدوية" },
            new SubCategory { Id = (int)SubCategoryType.CoinsAndStamps, CategoryId = (int)CategoryType.Antiques, Name = "Coins & Stamps", NameAr = "عملات وطوابع" },

            new SubCategory { Id = (int)SubCategoryType.MenClothing, CategoryId = (int)CategoryType.Clothing, Name = "Men Clothing", NameAr = "ملابس رجالي" },
            new SubCategory { Id = (int)SubCategoryType.WomenClothing, CategoryId = (int)CategoryType.Clothing, Name = "Women Clothing", NameAr = "ملابس حريمي" },
            new SubCategory { Id = (int)SubCategoryType.KidsClothing, CategoryId = (int)CategoryType.Clothing, Name = "Kids Clothing", NameAr = "ملابس أطفال" },

            new SubCategory { Id = (int)SubCategoryType.Accessories, CategoryId = (int)CategoryType.OnlineShopping, Name = "Accessories", NameAr = "إكسسوارات" },
            new SubCategory { Id = (int)SubCategoryType.Cosmetics, CategoryId = (int)CategoryType.OnlineShopping, Name = "Cosmetics", NameAr = "مستحضرات التجميل" },
            new SubCategory { Id = (int)SubCategoryType.HomeAndKitchen, CategoryId = (int)CategoryType.OnlineShopping, Name = "Home & Kitchen", NameAr = "المنزل والمطبخ" },
            new SubCategory { Id = (int)SubCategoryType.ShoppingElectronics, CategoryId = (int)CategoryType.OnlineShopping, Name = "Electronics", NameAr = "إلكترونيات" },
            new SubCategory { Id = (int)SubCategoryType.GiftsAndToys, CategoryId = (int)CategoryType.OnlineShopping, Name = "Gifts & Toys", NameAr = "هدايا وألعاب" },
            new SubCategory { Id = (int)SubCategoryType.HomemadeFood, CategoryId = (int)CategoryType.OnlineShopping, Name = "Homemade Food", NameAr = "أكل منزلي" },

            new SubCategory { Id = (int)SubCategoryType.Furniture, CategoryId = (int)CategoryType.HomeFurnishing, Name = "Furniture", NameAr = "أثاث" },
            new SubCategory { Id = (int)SubCategoryType.FurnishingsAndCurtains, CategoryId = (int)CategoryType.HomeFurnishing, Name = "Furnishings & Curtains", NameAr = "مفروشات وستائر" },
            new SubCategory { Id = (int)SubCategoryType.LightingAndDecor, CategoryId = (int)CategoryType.HomeFurnishing, Name = "Lighting & Decor", NameAr = "إضاءة وديكور" },
            new SubCategory { Id = (int)SubCategoryType.KitchenTools, CategoryId = (int)CategoryType.HomeFurnishing, Name = "Kitchen Tools", NameAr = "أدوات المطبخ" },
            new SubCategory { Id = (int)SubCategoryType.HomeAppliances, CategoryId = (int)CategoryType.HomeFurnishing, Name = "Home Appliances", NameAr = "أجهزة كهربائية منزلية" },
            new SubCategory { Id = (int)SubCategoryType.BathroomSupplies, CategoryId = (int)CategoryType.HomeFurnishing, Name = "Bathroom Supplies", NameAr = "مستلزمات الحمام" },
            new SubCategory { Id = (int)SubCategoryType.PlantsAndOrnaments, CategoryId = (int)CategoryType.HomeFurnishing, Name = "Plants & Ornaments", NameAr = "نباتات وزينة" },

            new SubCategory { Id = (int)SubCategoryType.Lands, CategoryId = (int)CategoryType.RealEstate, Name = "Lands", NameAr = "أراضي" },
            new SubCategory { Id = (int)SubCategoryType.Apartments, CategoryId = (int)CategoryType.RealEstate, Name = "Apartments", NameAr = "شقق" },
            new SubCategory { Id = (int)SubCategoryType.Shops, CategoryId = (int)CategoryType.RealEstate, Name = "Shops", NameAr = "محلات" },

            new SubCategory { Id = (int)SubCategoryType.Rescues, CategoryId = (int)CategoryType.Charity, Name = "Rescues", NameAr = "الاستغاثة" },
            new SubCategory { Id = (int)SubCategoryType.BloodRequests, CategoryId = (int)CategoryType.Charity, Name = "Blood Requests", NameAr = "فصائل الدم" },
            new SubCategory { Id = (int)SubCategoryType.AskConsults, CategoryId = (int)CategoryType.Charity, Name = "Ask & Consult", NameAr = "اسأل واستشير" }
        );
    }
}
