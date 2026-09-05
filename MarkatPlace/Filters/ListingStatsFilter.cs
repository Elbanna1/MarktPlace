using System.Collections;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace MarkatPlace.Filters;

public class ListingStatsFilter : IAsyncResultFilter
{
    private const int MaxNodes = 20_000;

    private static readonly Dictionary<Type, PropertyInfo[]> PropertyCache = new();
    private static readonly ReaderWriterLockSlim CacheLock = new();

    private readonly ILogger<ListingStatsFilter> _logger;

    public ListingStatsFilter(ILogger<ListingStatsFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult { Value: { } payload })
        {
            if (IsCreated(context))
            {
                ZeroStats(payload);
                await next();
                return;
            }

            try
            {
                await FillAsync(context, payload);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Filling listing stats failed; serving the response without them.");
            }
        }

        await next();
    }

    private static bool IsCreated(ResultExecutingContext context) =>
        (context.Result as IStatusCodeActionResult)?.StatusCode == StatusCodes.Status201Created;

    private void ZeroStats(object payload)
    {
        try
        {
            var targets = new List<IListingViews>();
            Collect(payload, targets, new HashSet<object>(ReferenceEqualityComparer.Instance), new Counter());

            foreach (var target in targets)
                target.Views = 0;

            foreach (var target in targets.OfType<IListingStats>())
            {
                target.IsFavorite = false;
                target.AverageRating = null;
                target.RatingsCount = 0;
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Zeroing the stats of a newly created listing failed.");
        }
    }

    private async Task FillAsync(ResultExecutingContext context, object payload)
    {
        var targets = new List<IListingViews>();
        Collect(payload, targets, new HashSet<object>(ReferenceEqualityComparer.Instance), new Counter());

        if (targets.Count == 0)
            return;

        var stats = targets.OfType<IListingStats>().ToList();

        var interactions = context.HttpContext.RequestServices
            .GetRequiredService<IListingInteractionService>();

        var viewerUserId = CurrentUserId(context);

        if (stats.Count > 0)
        {
            var keys = stats
                .Select(t => (t.StatsListingType, t.Id))
                .Distinct()
                .ToList();

            var counters = await interactions.GetCountersAsync(
                keys, viewerUserId, context.HttpContext.RequestAborted);

            foreach (var target in stats)
            {
                if (!counters.TryGetValue((target.StatsListingType, target.Id), out var counter) || counter is null)
                {
                    target.Views = 0;
                    target.IsFavorite = false;
                    continue;
                }

                target.Views = counter.Views;
                target.AverageRating = counter.AverageRating;
                target.RatingsCount = counter.RatingsCount;
                target.IsFavorite = counter.IsFavorite;
            }
        }

        await ApplyViewVisibilityAsync(context, targets, viewerUserId, interactions);
    }

    private static async Task ApplyViewVisibilityAsync(
        ResultExecutingContext context,
        IReadOnlyList<IListingViews> targets,
        string? viewerUserId,
        IListingInteractionService interactions)
    {
        if (context.HttpContext.User?.IsInRole(AppRoles.Admin) == true)
            return;

        if (string.IsNullOrEmpty(viewerUserId))
        {
            foreach (var target in targets)
                target.Views = null;

            return;
        }

        var keys = targets
            .Select(t => (t.ViewsListingType, t.ViewsListingId))
            .Distinct()
            .ToList();

        var owned = await interactions.GetOwnedAsync(
            keys, viewerUserId, context.HttpContext.RequestAborted);

        foreach (var target in targets)
        {
            if (!owned.Contains((target.ViewsListingType, target.ViewsListingId)))
                target.Views = null;
        }
    }

    private static string? CurrentUserId(ResultExecutingContext context)
    {
        var user = context.HttpContext.User;

        if (user?.Identity?.IsAuthenticated != true)
            return null;

        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);

        return string.IsNullOrEmpty(id) ? null : id;
    }

    private static void Collect(object? node, List<IListingViews> found, HashSet<object> seen, Counter budget)
    {
        if (node is null || budget.Exceeded)
            return;

        if (node is string || node.GetType().IsPrimitive)
            return;

        if (!seen.Add(node))
            return;

        if (++budget.Value > MaxNodes)
            return;

        if (node is IListingViews views)
            found.Add(views);

        if (node is IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                Collect(item, found, seen, budget);

                if (budget.Exceeded)
                    return;
            }

            return;
        }

        foreach (var property in PropertiesOf(node.GetType()))
        {
            object? value;

            try
            {
                value = property.GetValue(node);
            }
            catch
            {
                continue;
            }

            Collect(value, found, seen, budget);

            if (budget.Exceeded)
                return;
        }
    }

    private static PropertyInfo[] PropertiesOf(Type type)
    {
        CacheLock.EnterReadLock();
        try
        {
            if (PropertyCache.TryGetValue(type, out var cached))
                return cached;
        }
        finally
        {
            CacheLock.ExitReadLock();
        }

        var properties = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.GetIndexParameters().Length == 0 && IsWorthOpening(p.PropertyType))
            .ToArray();

        CacheLock.EnterWriteLock();
        try
        {
            PropertyCache[type] = properties;
        }
        finally
        {
            CacheLock.ExitWriteLock();
        }

        return properties;
    }

    private static bool IsWorthOpening(Type type)
    {
        if (type == typeof(string) || type.IsPrimitive || type.IsEnum)
            return false;

        if (typeof(IListingViews).IsAssignableFrom(type))
            return true;

        if (typeof(IEnumerable).IsAssignableFrom(type))
            return true;

        var name = type.Namespace;

        return name is not null &&
               (name.StartsWith("Shared.", StringComparison.Ordinal) ||
                name.StartsWith("Domain.", StringComparison.Ordinal));
    }

    private sealed class Counter
    {
        public int Value;

        public bool Exceeded => Value > MaxNodes;
    }
}
