using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class LostFoundCommentConfiguration : IEntityTypeConfiguration<LostFoundComment>
{
    public void Configure(EntityTypeBuilder<LostFoundComment> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Comment).IsRequired().HasMaxLength(1000);
        builder.Property(c => c.UserId).IsRequired();

        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(c => new { c.PostId, c.CreatedAt });

        builder.HasQueryFilter(c => !c.Post.IsDeleted);
    }
}
