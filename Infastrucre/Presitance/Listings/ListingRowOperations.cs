using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Data;
using Shared.Enums;

namespace Persistence.Listings;

public static class ListingRowOperations
{
    public static async Task<T> RunExclusiveAsync<T>(
        AppDbContext context,
        ListingModuleType type,
        Guid id,
        Func<Task<T>> operation,
        CancellationToken cancellationToken)
    {
        if (!context.Database.IsRelational() ||
            !ModerationModuleRegistry.EntityTypes.TryGetValue(type, out var clrType))
        {
            return await operation();
        }

        var lockSql = BuildLockSql(context, clrType);
        var strategy = context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            context.ChangeTracker.Clear();

            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            await context.Database.ExecuteSqlRawAsync(lockSql, new object[] { id }, cancellationToken);

            var result = await operation();

            await transaction.CommitAsync(cancellationToken);

            return result;
        });
    }

    public static void MarkAsNew(AppDbContext context, object listing, DateTime utcNow)
    {
        var entry = context.Entry(listing);

        if (entry.Metadata.FindProperty("CreatedAt") is { } property && property.ClrType == typeof(DateTime))
            entry.Property("CreatedAt").CurrentValue = utcNow;
    }

    private static string BuildLockSql(AppDbContext context, Type clrType)
    {
        var entityType = context.Model.FindEntityType(clrType)
            ?? throw new InvalidOperationException($"{clrType.Name} is not part of the model.");

        var table = entityType.GetTableName()
            ?? throw new InvalidOperationException($"{clrType.Name} has no table.");

        var key = entityType.FindPrimaryKey()!.Properties.Single();

        var column = key.GetColumnName(StoreObjectIdentifier.Table(table, entityType.GetSchema()))
            ?? throw new InvalidOperationException($"{clrType.Name} has no key column.");

        var sql = context.GetService<ISqlGenerationHelper>();

        return $"SELECT TOP(1) 1 FROM {sql.DelimitIdentifier(table, entityType.GetSchema())} " +
               $"WITH (UPDLOCK, ROWLOCK) WHERE {sql.DelimitIdentifier(column)} = {{0}}";
    }
}
