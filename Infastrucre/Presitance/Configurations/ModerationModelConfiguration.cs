using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Enums;

namespace Persistence.Configurations;

public static class ModerationModelConfiguration
{
    public const string FilterName = QueryFilterNames.Moderation;

    private const string CreatedAtProperty = "CreatedAt";

    private const string IsDeletedProperty = "IsDeleted";

    private const string ExpireAtProperty = "ExpireAt";

    private static readonly string[] PromotionProperties = { "IsPremium", "IsFeatured" };

    private static string[] OrderingIndexKey<TEntity>(
        Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TEntity> entity)
        where TEntity : class
    {
        var key = new List<string> { nameof(IModeratedListing.ModerationStatus) };

        if (PromotionProperties.All(name => entity.Metadata.FindProperty(name) is not null))
            key.AddRange(PromotionProperties);

        if (entity.Metadata.FindProperty(CreatedAtProperty) is not null)
            key.Add(CreatedAtProperty);

        if (entity.Metadata.FindPrimaryKey() is { Properties.Count: 1 } primaryKey &&
            primaryKey.Properties[0].ClrType == typeof(Guid))
        {
            key.Add(primaryKey.Properties[0].Name);
        }

        return key.ToArray();
    }

    private static readonly MethodInfo ConfigureMethod =
        typeof(ModerationModelConfiguration)
            .GetMethod(nameof(Configure), BindingFlags.NonPublic | BindingFlags.Static)!;

    private static readonly MethodInfo ConfigureExpiringMethod =
        typeof(ModerationModelConfiguration)
            .GetMethod(nameof(ConfigureExpiring), BindingFlags.NonPublic | BindingFlags.Static)!;

    public static void ApplyModerationConfiguration(this ModelBuilder builder)
    {
        var moderated = builder.Model
            .GetEntityTypes()
            .Where(entityType =>
                !entityType.IsOwned() &&
                entityType.ClrType is { IsClass: true, IsAbstract: false } &&
                typeof(IModeratedListing).IsAssignableFrom(entityType.ClrType))
            .Select(entityType => entityType.ClrType)
            .Distinct()
            .ToList();

        foreach (var clrType in moderated)
        {
            var method = typeof(IExpiringListing).IsAssignableFrom(clrType)
                ? ConfigureExpiringMethod
                : ConfigureMethod;

            method.MakeGenericMethod(clrType).Invoke(null, new object[] { builder });
        }
    }

    private static void ConfigureExpiring<TEntity>(ModelBuilder builder)
        where TEntity : class, IModeratedListing, IExpiringListing
    {
        var entity = builder.Entity<TEntity>();

        ConfigureModerationColumns(entity, withModerationIndex: false);

        entity.Property(x => x.PublishedAt);
        entity.Property(x => x.ExpireAt);
        entity.Property(x => x.FirstPublishedAt);
        entity.Property(x => x.RepublishCount).HasDefaultValue(0);

        entity.HasIndex(x => x.ExpireAt);

        var orderingKey = OrderingIndexKey(entity);

        var visibility = entity.HasIndex(orderingKey)
            .IsDescending(orderingKey.Select((_, position) => position > 0).ToArray());

        var carried = new[] { IsDeletedProperty, ExpireAtProperty }
            .Where(name => entity.Metadata.FindProperty(name) is not null)
            .ToArray();

        if (carried.Length > 0)
            visibility.IncludeProperties(carried);

        entity.HasQueryFilter(FilterName, x =>
            x.ModerationStatus == ModerationStatus.Approved &&
            (x.ExpireAt == null || x.ExpireAt > DateTime.UtcNow));
    }

    private static void Configure<TEntity>(ModelBuilder builder)
        where TEntity : class, IModeratedListing
    {
        var entity = builder.Entity<TEntity>();

        ConfigureModerationColumns(entity);

        entity.HasQueryFilter(FilterName, x => x.ModerationStatus == ModerationStatus.Approved);
    }

    private static void ConfigureModerationColumns<TEntity>(
        Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TEntity> entity,
        bool withModerationIndex = true)
        where TEntity : class, IModeratedListing
    {
        entity.Property(x => x.ModerationStatus)
            .IsRequired()
            .HasDefaultValue(ModerationStatus.Pending);

        entity.Property(x => x.ModeratedAt);

        entity.Property(x => x.ModeratedBy)
            .HasMaxLength(450);

        entity.Property(x => x.RejectionReason);

        entity.Property(x => x.ModerationNotes)
            .HasMaxLength(ModerationCatalog.MaxNotesLength);

        if (withModerationIndex && !LeadsWithModerationStatus(entity.Metadata))
            entity.HasIndex(x => x.ModerationStatus);
    }

    private static bool LeadsWithModerationStatus(
        Microsoft.EntityFrameworkCore.Metadata.IReadOnlyEntityType entityType) =>
        entityType.GetIndexes().Any(index =>
            index.Properties.Count > 0 &&
            index.Properties[0].Name == nameof(IModeratedListing.ModerationStatus));
}
