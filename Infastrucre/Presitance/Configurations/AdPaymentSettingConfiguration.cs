using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class AdPaymentSettingConfiguration : IEntityTypeConfiguration<AdPaymentSetting>
{
    public const int SingletonId = 1;

    public void Configure(EntityTypeBuilder<AdPaymentSetting> builder)
    {
        builder.ToTable("AdPaymentSettings", table =>
        {
            table.HasCheckConstraint("CK_AdPaymentSettings_SingleRow", $"[Id] = {SingletonId}");
            table.HasCheckConstraint("CK_AdPaymentSettings_Price", "[Price] >= 0");
        });

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedNever();

        builder.Property(s => s.Price).HasColumnType("decimal(18,2)");
        builder.Property(s => s.Currency).IsRequired().HasMaxLength(3);
        builder.Property(s => s.Instructions).HasMaxLength(1000);
        builder.Property(s => s.PaymentMethodIds).HasMaxLength(500);
        builder.Property(s => s.UpdatedBy).HasMaxLength(450);

        builder.HasData(new AdPaymentSetting
        {
            Id = SingletonId,
            IsEnabled = false,
            Price = 0m,
            Currency = PaymentCatalog.DefaultCurrency
        });
    }
}
