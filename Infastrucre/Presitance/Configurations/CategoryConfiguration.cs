using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

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

        builder.HasData(CategoryCatalog.Categories.Select(entry => new Category
        {
            Id = (int)entry.Id,
            Name = entry.Name,
            NameAr = entry.NameAr
        }));
    }
}
