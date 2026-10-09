using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

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

        builder.HasData(CategoryCatalog.SubCategories.Select(entry => new SubCategory
        {
            Id = (int)entry.Id,
            CategoryId = (int)entry.Category,
            Name = entry.Name,
            NameAr = entry.NameAr
        }));
    }
}
