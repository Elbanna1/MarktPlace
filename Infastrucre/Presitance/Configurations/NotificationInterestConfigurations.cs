using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class UserNotificationInterestConfiguration : IEntityTypeConfiguration<UserNotificationInterest>
{
    public void Configure(EntityTypeBuilder<UserNotificationInterest> builder)
    {
        builder.ToTable("UserNotificationInterests");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.IsEnabled).HasDefaultValue(true);

        builder.HasIndex(i => new { i.UserId, i.CategoryId, i.SubCategoryId })
            .IsUnique()
            .HasFilter(null)
            .HasDatabaseName("IX_UserNotificationInterests_User_Category_SubCategory");

        builder.HasIndex(i => new { i.CategoryId, i.SubCategoryId, i.IsEnabled })
            .HasDatabaseName("IX_UserNotificationInterests_Category_SubCategory_Enabled");

        builder.HasOne(i => i.User)
            .WithMany()
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public class UserNotificationPreferenceConfiguration : IEntityTypeConfiguration<UserNotificationPreference>
{
    public void Configure(EntityTypeBuilder<UserNotificationPreference> builder)
    {
        builder.ToTable("UserNotificationPreferences");

        builder.HasKey(p => p.UserId);

        builder.Property(p => p.UserId).HasMaxLength(450);

        builder.Property(p => p.NewListingsEnabled).HasDefaultValue(true);

        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public class ListingNotificationDispatchConfiguration : IEntityTypeConfiguration<ListingNotificationDispatch>
{
    public void Configure(EntityTypeBuilder<ListingNotificationDispatch> builder)
    {
        builder.ToTable("ListingNotificationDispatches");

        builder.HasKey(d => new { d.ListingType, d.ListingId });

        builder.Property(d => d.ListingType).HasConversion<int>();
        builder.Property(d => d.RecipientCount).HasDefaultValue(0);
    }
}
