using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class LostFoundPostConfiguration : IEntityTypeConfiguration<LostFoundPost>
{
    public void Configure(EntityTypeBuilder<LostFoundPost> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(150);
        builder.Property(p => p.ItemName).IsRequired().HasMaxLength(150);
        builder.Property(p => p.Description).IsRequired().HasMaxLength(4000);
        builder.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(20);
        builder.Property(p => p.Governorate).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Center).IsRequired().HasMaxLength(50);
        builder.Property(p => p.UserId).IsRequired();

        builder.Property(p => p.PostType).HasConversion<int>();
        builder.Property(p => p.Status).HasConversion<int>();

        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(p => p.Images)
            .WithOne(i => i.Post)
            .HasForeignKey(i => i.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Likes)
            .WithOne(l => l.Post)
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, p => !p.IsDeleted);

        builder.Property(p => p.LostDate);
        builder.Property(p => p.FoundDate);

        builder.HasIndex(p => p.CreatedAt);
        builder.HasIndex(p => p.PostType);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.UserId);
        builder.HasIndex(p => p.LostDate);
        builder.HasIndex(p => p.FoundDate);
    }
}
