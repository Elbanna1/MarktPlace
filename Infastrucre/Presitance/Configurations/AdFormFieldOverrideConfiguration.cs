using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class AdFormFieldOverrideConfiguration : IEntityTypeConfiguration<AdFormFieldOverride>
{
    public void Configure(EntityTypeBuilder<AdFormFieldOverride> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.FieldName).IsRequired().HasMaxLength(100);
        builder.Property(f => f.Label).HasMaxLength(200);
        builder.Property(f => f.LabelEn).HasMaxLength(200);
        builder.Property(f => f.Type).HasMaxLength(30);
        builder.Property(f => f.Section).HasMaxLength(150);
        builder.Property(f => f.Placeholder).HasMaxLength(300);
        builder.Property(f => f.HelpText).HasMaxLength(500);
        builder.Property(f => f.Pattern).HasMaxLength(500);
        builder.Property(f => f.PatternMessage).HasMaxLength(300);

        builder.Property(f => f.MinValue).HasPrecision(18, 2);
        builder.Property(f => f.MaxValue).HasPrecision(18, 2);

        builder.Property(f => f.IsCustom).IsRequired().HasDefaultValue(false);
        builder.Property(f => f.IsHidden).IsRequired().HasDefaultValue(false);

        builder.Property(f => f.VisibleWhenField).HasMaxLength(100);
        builder.Property(f => f.VisibleWhenValues).HasMaxLength(2000);
        builder.Property(f => f.RequiredWhenField).HasMaxLength(100);
        builder.Property(f => f.RequiredWhenValues).HasMaxLength(2000);

        builder.Property(f => f.UpdatedBy).HasMaxLength(450);

        builder.HasOne(f => f.Category)
            .WithMany()
            .HasForeignKey(f => f.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.SubCategory)
            .WithMany()
            .HasForeignKey(f => f.SubCategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(f => new { f.CategoryId, f.SubCategoryId, f.FieldName }).IsUnique();
    }
}

public class AdFormFieldOptionConfiguration : IEntityTypeConfiguration<AdFormFieldOption>
{
    public void Configure(EntityTypeBuilder<AdFormFieldOption> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Value).IsRequired().HasMaxLength(200);
        builder.Property(o => o.Label).IsRequired().HasMaxLength(200);
        builder.Property(o => o.LabelEn).HasMaxLength(200);
        builder.Property(o => o.Group).HasMaxLength(150);
        builder.Property(o => o.SortOrder).IsRequired().HasDefaultValue(0);
        builder.Property(o => o.IsActive).IsRequired().HasDefaultValue(true);

        builder.HasOne(o => o.FieldOverride)
            .WithMany(f => f.Options)
            .HasForeignKey(o => o.FieldOverrideId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(o => new { o.FieldOverrideId, o.Value }).IsUnique();
    }
}
