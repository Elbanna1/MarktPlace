using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class AdvertisementConfiguration : IEntityTypeConfiguration<Advertisement>
{
    public void Configure(EntityTypeBuilder<Advertisement> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Title).IsRequired().HasMaxLength(150);
        builder.Property(a => a.Description).IsRequired().HasMaxLength(4000);
        builder.Property(a => a.Price).HasColumnType("decimal(18,2)");

        builder.Property(a => a.Governorate).IsRequired().HasMaxLength(50);
        builder.Property(a => a.Center).IsRequired().HasMaxLength(50);
        builder.Property(a => a.PhoneNumber).IsRequired().HasMaxLength(20);

        builder.Property(a => a.Brand).HasMaxLength(100).IsRequired(false);
        builder.Property(a => a.Model).HasMaxLength(100).IsRequired(false);
        builder.Property(a => a.OtherBrand).HasMaxLength(100);
        builder.Property(a => a.Color).HasMaxLength(50);
        builder.Property(a => a.OtherColor).HasMaxLength(50);

        builder.Property(a => a.DetailedAddress).HasMaxLength(300);
        builder.Property(a => a.VideoPath).HasMaxLength(500);
        builder.Property(a => a.VideoUrl).HasMaxLength(1000);
        builder.Property(a => a.OtherMachineType).HasMaxLength(100);
        builder.Property(a => a.OtherVehicleType).HasMaxLength(100);
        builder.Property(a => a.BucketCapacity).HasMaxLength(100);
        builder.Property(a => a.InspectionLocation).HasMaxLength(300);
        builder.Property(a => a.Route).HasMaxLength(300);
        builder.Property(a => a.ConditionReport).HasMaxLength(4000);
        builder.Property(a => a.ExchangeDetails).HasMaxLength(4000);

        builder.Property(a => a.DownPayment).HasColumnType("decimal(18,2)");
        builder.Property(a => a.MonthlyInstallment).HasColumnType("decimal(18,2)");
        builder.Property(a => a.HourlyPrice).HasColumnType("decimal(18,2)");
        builder.Property(a => a.DailyPrice).HasColumnType("decimal(18,2)");
        builder.Property(a => a.WeeklyPrice).HasColumnType("decimal(18,2)");
        builder.Property(a => a.MonthlyPrice).HasColumnType("decimal(18,2)");
        builder.Property(a => a.DepositAmount).HasColumnType("decimal(18,2)");
        builder.Property(a => a.DifferenceAmount).HasColumnType("decimal(18,2)");
        builder.Property(a => a.PowerValue).HasColumnType("decimal(10,2)");
        builder.Property(a => a.OperatingWeightTons).HasColumnType("decimal(10,2)");

        builder.Property(a => a.BusinessName).HasMaxLength(150);
        builder.Property(a => a.Address).HasMaxLength(300);
        builder.Property(a => a.GoogleMapsUrl).HasMaxLength(1000);
        builder.Property(a => a.WhatsApp).HasMaxLength(20);
        builder.Property(a => a.Email).HasMaxLength(256);
        builder.Property(a => a.OtherProductionSpecialty).HasMaxLength(150);
        builder.Property(a => a.OtherFarmType).HasMaxLength(150);
        builder.Property(a => a.OtherCompanyField).HasMaxLength(150);
        builder.Property(a => a.OtherSupplierType).HasMaxLength(150);
        builder.Property(a => a.OtherTradeType).HasMaxLength(150);

        builder.Property(a => a.OwnerId).IsRequired();

        builder.Property(a => a.ListingType).HasConversion<int>();
        builder.Property(a => a.Status).HasConversion<int>();
        builder.Property(a => a.Transmission).HasConversion<int>();
        builder.Property(a => a.FuelType).HasConversion<int>();
        builder.Property(a => a.Condition).HasConversion<int>();
        builder.Property(a => a.BodyType).HasConversion<int>();
        builder.Property(a => a.CoolingType).HasConversion<int>();
        builder.Property(a => a.DamageLevel).HasConversion<int>();
        builder.Property(a => a.InterestedIn).HasConversion<int>();
        builder.Property(a => a.TechnicalCondition).HasConversion<int>();
        builder.Property(a => a.OriginCountry).HasConversion<int>();
        builder.Property(a => a.AssemblyCountry).HasConversion<int>();
        builder.Property(a => a.PreviousOwners).HasConversion<int>();
        builder.Property(a => a.UsageType).HasConversion<int>();
        builder.Property(a => a.AccidentsCount).HasConversion<int>();
        builder.Property(a => a.LicenseStatus).HasConversion<int>();
        builder.Property(a => a.InsuranceType).HasConversion<int>();
        builder.Property(a => a.MotorcycleType).HasConversion<int>();
        builder.Property(a => a.StartType).HasConversion<int>();
        builder.Property(a => a.MachineType).HasConversion<int>();
        builder.Property(a => a.PowerUnit).HasConversion<int>();
        builder.Property(a => a.DriveSystem).HasConversion<int>();
        builder.Property(a => a.VehicleType).HasConversion<int>();
        builder.Property(a => a.ActivityType).HasConversion<int>();
        builder.Property(a => a.OperatingLicenseStatus).HasConversion<int>();
        builder.Property(a => a.ProductionSpecialty).HasConversion<int>();
        builder.Property(a => a.FarmType).HasConversion<int>();
        builder.Property(a => a.FarmingMethod).HasConversion<int>();
        builder.Property(a => a.AvailabilitySeason).HasConversion<int>();
        builder.Property(a => a.CompanyField).HasConversion<int>();
        builder.Property(a => a.SupplierType).HasConversion<int>();
        builder.Property(a => a.TradeType).HasConversion<int>();
        builder.Property(a => a.SaleType).HasConversion<int>();

        builder.HasOne(a => a.Owner)
            .WithMany()
            .HasForeignKey(a => a.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Category)
            .WithMany(c => c.Advertisements)
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.SubCategory)
            .WithMany(s => s.Advertisements)
            .HasForeignKey(a => a.SubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => a.Status);
        builder.HasIndex(a => a.OwnerId);
        builder.HasIndex(a => a.ExpireAt);
        builder.HasIndex(a => a.CreatedAt);
        builder.HasIndex(a => new { a.CategoryId, a.SubCategoryId });

        builder.HasIndex(a => new { a.ModerationStatus, a.Status, a.CreatedAt })
            .IsDescending(false, false, true)
            .IncludeProperties(a => new { a.ExpireAt, a.CategoryId, a.SubCategoryId, a.Price, a.Views });

        builder.HasIndex(a => new { a.ModerationStatus, a.Status, a.Views, a.CreatedAt })
            .IsDescending(false, false, true, true)
            .IncludeProperties(a => new { a.ExpireAt, a.CategoryId, a.SubCategoryId, a.Price });

        builder.HasIndex(
                a => new { a.ModerationStatus, a.Status, a.Price, a.CreatedAt },
                "IX_Advertisements_Feed_PriceAsc")
            .IsDescending(false, false, false, true)
            .IncludeProperties(a => new { a.ExpireAt, a.CategoryId, a.SubCategoryId, a.Views });

        builder.HasIndex(
                a => new { a.ModerationStatus, a.Status, a.Price, a.CreatedAt },
                "IX_Advertisements_Feed_PriceDesc")
            .IsDescending(false, false, true, true)
            .IncludeProperties(a => new { a.ExpireAt, a.CategoryId, a.SubCategoryId, a.Views });
    }
}
