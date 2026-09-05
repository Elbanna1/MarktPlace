using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("PaymentMethods");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name).IsRequired().HasMaxLength(100);
        builder.Property(m => m.ArabicName).HasMaxLength(100);
        builder.Property(m => m.PhoneNumber).HasMaxLength(20);
        builder.Property(m => m.InstaPayIdentifier).HasMaxLength(100);
        builder.Property(m => m.BankName).HasMaxLength(150);
        builder.Property(m => m.AccountHolderName).HasMaxLength(150);
        builder.Property(m => m.AccountNumber).HasMaxLength(50);
        builder.Property(m => m.Iban).HasMaxLength(50);
        builder.Property(m => m.Instructions).HasMaxLength(1000);

        builder.Property(m => m.Type).HasConversion<int>();

        builder.HasIndex(m => new { m.IsActive, m.DisplayOrder });
        builder.HasIndex(m => m.Name).IsUnique();

        builder.HasData(PaymentCatalog.SeedMethods.Select(seed => new PaymentMethod
        {
            Id = seed.Id,
            Name = seed.Name,
            ArabicName = seed.ArabicName,
            Type = seed.Type,
            PhoneNumber = seed.PhoneNumber,
            InstaPayIdentifier = seed.InstaPayIdentifier,
            BankName = seed.BankName,
            AccountHolderName = seed.AccountHolderName,
            AccountNumber = seed.AccountNumber,
            Iban = seed.Iban,
            Instructions = seed.Instructions,
            DisplayOrder = seed.DisplayOrder,
            IsActive = true,
            CreatedAt = PaymentCatalog.SeedTimestamp
        }));
    }
}
