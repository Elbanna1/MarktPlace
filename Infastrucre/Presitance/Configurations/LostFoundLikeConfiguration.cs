using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class LostFoundLikeConfiguration : IEntityTypeConfiguration<LostFoundLike>
{
    public void Configure(EntityTypeBuilder<LostFoundLike> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.UserId).IsRequired();

        builder.HasOne(l => l.User)
            .WithMany()
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(l => new { l.PostId, l.UserId }).IsUnique();

        builder.HasQueryFilter(l => !l.Post.IsDeleted);
    }
}
