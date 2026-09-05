using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class AdminAuditLogConfiguration : IEntityTypeConfiguration<AdminAuditLog>
{
    public void Configure(EntityTypeBuilder<AdminAuditLog> builder)
    {
        builder.ToTable("AdminAuditLogs");

        builder.HasKey(log => log.Id);

        builder.Property(log => log.AdminUserId).IsRequired().HasMaxLength(450);
        builder.Property(log => log.AdminName).HasMaxLength(200);

        builder.Property(log => log.Action).IsRequired();

        builder.Property(log => log.TargetType).IsRequired().HasMaxLength(60);
        builder.Property(log => log.TargetId).HasMaxLength(100);

        builder.Property(log => log.Description)
            .IsRequired()
            .HasMaxLength(AdminAuditCatalog.MaxDescriptionLength);

        builder.Property(log => log.OldValue).HasMaxLength(AdminAuditCatalog.MaxValueLength);
        builder.Property(log => log.NewValue).HasMaxLength(AdminAuditCatalog.MaxValueLength);

        builder.Property(log => log.IpAddress).HasMaxLength(45);

        builder.Property(log => log.CreatedAt).IsRequired();

        builder.HasIndex(log => log.CreatedAt);

        builder.HasIndex(log => new { log.AdminUserId, log.CreatedAt });
        builder.HasIndex(log => new { log.Action, log.CreatedAt });
        builder.HasIndex(log => new { log.TargetType, log.CreatedAt });

        builder.HasIndex(log => new { log.TargetType, log.TargetId });
    }
}
