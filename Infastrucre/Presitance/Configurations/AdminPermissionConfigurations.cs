using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class AdminPageGrantConfiguration : IEntityTypeConfiguration<AdminPageGrant>
{
    public void Configure(EntityTypeBuilder<AdminPageGrant> builder)
    {
        builder.ToTable("AdminPageGrants");

        builder.HasKey(grant => grant.Id);

        builder.Property(grant => grant.AdminUserId).IsRequired().HasMaxLength(450);

        builder.Property(grant => grant.PageKey).IsRequired().HasMaxLength(60);

        builder.Property(grant => grant.GrantedBy).HasMaxLength(450);

        builder.Property(grant => grant.CreatedAt).IsRequired();

        builder.HasOne(grant => grant.AdminUser)
            .WithMany()
            .HasForeignKey(grant => grant.AdminUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(grant => new { grant.AdminUserId, grant.PageKey })
            .IsUnique()
            .HasDatabaseName("IX_AdminPageGrants_AdminUserId_PageKey");

        builder.HasIndex(grant => grant.PageKey);
    }
}

public class AdminPagePermissionConfiguration : IEntityTypeConfiguration<AdminPagePermission>
{
    public void Configure(EntityTypeBuilder<AdminPagePermission> builder)
    {
        builder.ToTable("AdminPagePermissions");

        builder.HasKey(permission => permission.Id);

        builder.Property(permission => permission.Permission).IsRequired();

        builder.Property(permission => permission.CreatedAt).IsRequired();

        builder.HasOne(permission => permission.Grant)
            .WithMany(grant => grant.Permissions)
            .HasForeignKey(permission => permission.AdminPageGrantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(permission => new { permission.AdminPageGrantId, permission.Permission })
            .IsUnique()
            .HasDatabaseName("IX_AdminPagePermissions_GrantId_Permission");
    }
}
