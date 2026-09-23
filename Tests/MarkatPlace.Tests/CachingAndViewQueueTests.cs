using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.ResponseCaching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Presentation.Filters;
using Services.Listings;
using ServicesAbstraction;
using Shared.Enums;
using Shared.Settings;
using Xunit;

namespace MarkatPlace.Tests;

/// <summary>
/// Covers the caching headers on the public reference endpoints and the in-process queue that took
/// the view bookkeeping off the details read.
/// </summary>
public class CachingAndViewQueueTests
{
    private static Task<HttpResponse> RunAsync(HttpCacheAttribute attribute, string? authorization) =>
        RunAsync(attribute, authorization, responseCaching: null);

    private static async Task<HttpResponse> RunAsync(
        HttpCacheAttribute attribute, string? authorization, IResponseCachingFeature? responseCaching)
    {
        var services = new ServiceCollection();
        services.AddOptions();
        services.AddLogging();
        services.AddControllers();

        var context = new DefaultHttpContext { RequestServices = services.BuildServiceProvider() };

        context.Request.Method = HttpMethods.Get;

        if (responseCaching is not null)
            context.Features.Set(responseCaching);

        if (authorization is not null)
            context.Request.Headers.Authorization = authorization;

        var actionContext = new ActionContext(context, new RouteData(), new ControllerActionDescriptor());

        var executing = new ResultExecutingContext(
            actionContext, [], new OkObjectResult(new { value = "قيمة" }), controller: new object());

        await attribute.OnResultExecutionAsync(
            executing,
            () => Task.FromResult(new ResultExecutedContext(
                actionContext, [], executing.Result, controller: new object())));

        return context.Response;
    }

    [Fact]
    public async Task Reference_data_is_cacheable_by_a_shared_cache_whoever_asks_for_it()
    {
        var attribute = new HttpCacheAttribute { MaxAgeSeconds = 300, Scope = HttpCacheScope.Shared };

        var anonymous = await RunAsync(attribute, authorization: null);
        var authenticated = await RunAsync(attribute, "Bearer token");

        Assert.Equal("public, max-age=300", anonymous.Headers.CacheControl);
        Assert.Equal("public, max-age=300", authenticated.Headers.CacheControl);
    }

    [Fact]
    public async Task A_viewer_specific_response_is_never_stored_in_a_shared_cache()
    {
        var attribute = new HttpCacheAttribute { MaxAgeSeconds = 300 };

        var authenticated = await RunAsync(attribute, "Bearer token");

        Assert.Equal("private, max-age=300", authenticated.Headers.CacheControl);

        // Without this a cache could hand an anonymous copy to a signed-in caller, or the reverse.
        Assert.Contains("Authorization", authenticated.Headers.Vary.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task Every_cached_response_varies_on_what_changes_its_bytes()
    {
        var response = await RunAsync(
            new HttpCacheAttribute { MaxAgeSeconds = 300, Scope = HttpCacheScope.Shared },
            authorization: null);

        var vary = response.Headers.Vary.ToString();

        Assert.Contains("Accept-Encoding", vary, StringComparison.Ordinal);
        Assert.Contains("Origin", vary, StringComparison.Ordinal);

        // A shared representation is identical for everyone, so pinning it to Authorization would
        // only fragment the cache.
        Assert.DoesNotContain("Authorization", vary, StringComparison.Ordinal);
    }

    [Fact]
    public async Task A_cached_response_is_keyed_on_its_query_string()
    {
        var feature = new StubResponseCachingFeature();

        await RunAsync(
            new HttpCacheAttribute { MaxAgeSeconds = 60, Scope = HttpCacheScope.Shared },
            authorization: null,
            feature);

        Assert.Equal(new[] { "*" }, feature.VaryByQueryKeys);
    }

    private sealed class StubResponseCachingFeature : IResponseCachingFeature
    {
        public string[]? VaryByQueryKeys { get; set; }
    }

    [Fact]
    public async Task A_cached_response_still_carries_an_entity_tag()
    {
        var response = await RunAsync(
            new HttpCacheAttribute { MaxAgeSeconds = 300, Scope = HttpCacheScope.Shared },
            authorization: null);

        Assert.StartsWith("W/\"", response.Headers.ETag.ToString(), StringComparison.Ordinal);
    }

    private static ListingViewQueue Queue(int dedupeWindowMinutes = 360) =>
        new(Options.Create(new ListingInteractionSettings
        {
            ViewDedupeWindowMinutes = dedupeWindowMinutes
        }), NullLogger<ListingViewQueue>.Instance);

    private static ListingViewEvent View(
        string? userId, string? ip, DateTime at, Guid? listingId = null) =>
        new(ListingModuleType.Land, listingId ?? Guid.Parse("11111111-1111-1111-1111-111111111111"),
            userId, ip, IncrementModuleCounter: true, at);

    [Fact]
    public void A_repeat_anonymous_view_inside_the_window_never_reaches_the_database()
    {
        var queue = Queue();
        var now = DateTime.UtcNow;

        Assert.True(queue.Record(View(userId: null, "203.0.113.7", now)));
        Assert.False(queue.Record(View(userId: null, "203.0.113.7", now.AddMinutes(1))));
        Assert.False(queue.Record(View(userId: null, "203.0.113.7", now.AddMinutes(359))));
    }

    [Fact]
    public void An_anonymous_view_counts_again_once_the_window_has_passed()
    {
        var queue = Queue();
        var now = DateTime.UtcNow;

        Assert.True(queue.Record(View(userId: null, "203.0.113.7", now)));
        Assert.True(queue.Record(View(userId: null, "203.0.113.7", now.AddMinutes(361))));
    }

    [Fact]
    public void Two_anonymous_viewers_of_the_same_listing_are_counted_separately()
    {
        var queue = Queue();
        var now = DateTime.UtcNow;

        Assert.True(queue.Record(View(userId: null, "203.0.113.7", now)));
        Assert.True(queue.Record(View(userId: null, "198.51.100.4", now)));
    }

    [Fact]
    public void One_anonymous_viewer_reading_two_listings_is_counted_on_both()
    {
        var queue = Queue();
        var now = DateTime.UtcNow;

        Assert.True(queue.Record(View(userId: null, "203.0.113.7", now, Guid.NewGuid())));
        Assert.True(queue.Record(View(userId: null, "203.0.113.7", now, Guid.NewGuid())));
    }

    [Fact]
    public void A_signed_in_view_is_always_queued_because_its_row_records_recently_viewed()
    {
        var queue = Queue();
        var now = DateTime.UtcNow;

        Assert.True(queue.Record(View("user-1", "203.0.113.7", now)));
        Assert.True(queue.Record(View("user-1", "203.0.113.7", now.AddSeconds(1))));
    }

    [Fact]
    public void A_view_with_neither_an_account_nor_an_address_is_discarded()
    {
        Assert.False(Queue().Record(View(userId: null, ip: null, DateTime.UtcNow)));
    }

    [Fact]
    public void The_queue_drops_rather_than_blocks_once_it_is_full()
    {
        var queue = Queue();
        var now = DateTime.UtcNow;

        for (var i = 0; i < ListingViewQueue.Capacity + 100; i++)
            queue.Record(View(userId: null, $"198.51.100.{i % 256}", now, Guid.NewGuid()));

        Assert.True(queue.Dropped > 0);
    }

    [Fact]
    public void The_details_read_hands_the_view_to_the_queue_instead_of_writing_it()
    {
        var filter = RepositoryRoot.ReadFile("MarkatPlace/Filters/ListingInteractionFilter.cs");

        Assert.Contains("IListingViewRecorder", filter, StringComparison.Ordinal);
        Assert.DoesNotContain("RecordViewAsync", filter, StringComparison.Ordinal);
    }

    [Fact]
    public void No_module_still_writes_its_own_view_counter_during_a_details_read()
    {
        var offenders = RepositoryRoot.SourceFiles()
            .Where(file => file.Contains("/Core/Services/", StringComparison.Ordinal))
            .Where(file => File.ReadAllText(file)
                .Contains("await _repository.IncrementViewCountAsync(id, cancellationToken);",
                    StringComparison.Ordinal))
            .Select(RepositoryRoot.Relative)
            .ToList();

        Assert.True(offenders.Count == 0,
            "These services still write on a GET: " + string.Join(", ", offenders));
    }

    [Fact]
    public void The_queue_is_drained_on_shutdown_rather_than_abandoned()
    {
        var service = RepositoryRoot.ReadFile("Core/Services/Listings/ListingViewFlushService.cs");

        Assert.Contains("public override async Task StopAsync", service, StringComparison.Ordinal);
        Assert.Contains("_queue.Complete();", service, StringComparison.Ordinal);
        Assert.Contains("ShutdownDrainBudget", service, StringComparison.Ordinal);
    }
}
