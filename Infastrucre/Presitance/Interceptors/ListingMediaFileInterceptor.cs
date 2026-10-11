using System.Runtime.CompilerServices;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using ServicesAbstraction;

namespace Persistence.Interceptors;

public sealed class ListingMediaFileInterceptor : SaveChangesInterceptor
{
    private sealed record PendingFiles(IReadOnlyList<string> Removed, IReadOnlyList<string> Added);

    private readonly ConditionalWeakTable<DbContext, PendingFiles> _pending = new();
    private readonly IFileService _files;
    private readonly ILogger<ListingMediaFileInterceptor> _logger;

    public ListingMediaFileInterceptor(IFileService files, ILogger<ListingMediaFileInterceptor> logger)
    {
        _files = files;
        _logger = logger;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        Capture(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        Capture(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        Complete(eventData.Context, succeeded: true);
        return base.SavedChanges(eventData, result);
    }

    public override ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        Complete(eventData.Context, succeeded: true);
        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    public override void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        Complete(eventData.Context, succeeded: false);
        base.SaveChangesFailed(eventData);
    }

    public override Task SaveChangesFailedAsync(
        DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        Complete(eventData.Context, succeeded: false);
        return base.SaveChangesFailedAsync(eventData, cancellationToken);
    }

    private void Capture(DbContext? context)
    {
        if (context is null)
            return;

        var removed = new List<string>();
        var added = new List<string>();

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.State is not (EntityState.Added or EntityState.Deleted))
                continue;

            var path = entry.Entity switch
            {
                IListingImage image => image.ImagePath,
                IListingVideo video => video.VideoPath,
                _ => null
            };

            if (string.IsNullOrWhiteSpace(path))
                continue;

            (entry.State == EntityState.Deleted ? removed : added).Add(path);
        }

        if (removed.Count == 0 && added.Count == 0)
        {
            _pending.Remove(context);
            return;
        }

        _pending.AddOrUpdate(context, new PendingFiles(removed, added));
    }

    private void Complete(DbContext? context, bool succeeded)
    {
        if (context is null || !_pending.TryGetValue(context, out var pending))
            return;

        _pending.Remove(context);

        foreach (var path in succeeded ? pending.Removed : pending.Added)
        {
            try
            {
                _files.Delete(path);
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
            {
                _logger.LogWarning(exception, "Could not delete the uploaded file {Path}.", path);
            }
        }
    }
}
