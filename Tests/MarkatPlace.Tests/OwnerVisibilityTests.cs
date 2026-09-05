using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Domain.Entities;
using Persistence.Configurations;
using Xunit;

namespace MarkatPlace.Tests;

public class OwnerVisibilityTests
{
    private static IEnumerable<string> RepositoryFiles() =>
        RepositoryRoot.SourceFiles()
            .Where(file => file.Contains("/Presitance/Repositories/", StringComparison.Ordinal))
            .Where(file => file.EndsWith("Repository.cs", StringComparison.Ordinal)
                        || file.EndsWith("Repositories.cs", StringComparison.Ordinal))
            .Where(file => !file.EndsWith("UserListingRepository.cs", StringComparison.Ordinal));

    [Fact]
    public void No_module_still_gates_its_by_id_read_on_includeUnmoderated_alone()
    {
        var offenders = new List<string>();

        foreach (var file in RepositoryFiles())
        {
            var source = File.ReadAllText(file);

            if (!source.Contains("GetByIdAsync", StringComparison.Ordinal))
                continue;

            if (Regex.IsMatch(source, @"if \(includeUnmoderated\)\s*\r?\n\s*query = query\.IncludingUnmoderated\(\);"))
                offenders.Add(RepositoryRoot.Relative(file));
        }

        offenders.Remove("Infastrucre/Presitance/Repositories/AdvertisementRepository.cs");

        Assert.True(offenders.Count == 0,
            "These modules answer 404 to a seller opening their own listing while it is قيد المراجعة. " +
            "Replace the block with `query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);`:"
            + Environment.NewLine + string.Join(Environment.NewLine, offenders));
    }

    [Fact]
    public void Every_module_by_id_read_goes_through_the_shared_owner_aware_rule()
    {
        var missing = new List<string>();

        foreach (var file in RepositoryFiles())
        {
            var name = Path.GetFileName(file);

            if (name == "AdvertisementRepository.cs")
                continue;

            var source = File.ReadAllText(file);

            if (!source.Contains("bool includeUnmoderated", StringComparison.Ordinal))
                continue;

            if (!source.Contains("VisibleToViewer(", StringComparison.Ordinal))
                missing.Add(RepositoryRoot.Relative(file));
        }

        Assert.True(missing.Count == 0,
            "These repositories take an `includeUnmoderated` flag but never apply the owner-visibility "
            + "rule, so their owners cannot open their own pending listings:"
            + Environment.NewLine + string.Join(Environment.NewLine, missing));
    }

    [Fact]
    public void Every_moderated_listing_with_a_publication_window_has_the_owner_column()
    {
        var missing = new List<string>();

        foreach (var type in ModeratedListingTypes())
        {
            if (!typeof(IExpiringListing).IsAssignableFrom(type))
                continue;

            var property = type.GetProperty(
                ModerationQueryExtensions.OwnerUserIdProperty,
                BindingFlags.Public | BindingFlags.Instance);

            if (property is null || property.PropertyType != typeof(string))
                missing.Add(type.Name);
        }

        Assert.True(missing.Count == 0,
            $"These listing entities have no mapped string '{ModerationQueryExtensions.OwnerUserIdProperty}' "
            + "property, so the owner-visibility rule cannot find their owner:"
            + Environment.NewLine + string.Join(", ", missing));
    }

    [Fact]
    public void The_moderated_listing_set_is_the_size_the_fix_was_written_for()
    {
        var all = ModeratedListingTypes().ToList();
        var windowed = all.Where(t => typeof(IExpiringListing).IsAssignableFrom(t)).ToList();

        Assert.True(all.Count >= 48, $"only {all.Count} moderated listing types were found.");
        Assert.Equal(all.Count - 1, windowed.Count);
    }

    [Fact]
    public void The_owner_rule_never_switches_off_the_soft_delete_filter()
    {
        var source = RepositoryRoot.ReadFile(
            "Infastrucre/Presitance/Configurations/ModerationQueryExtensions.cs");

        var rule = source[source.IndexOf("VisibleToViewer<TEntity>", StringComparison.Ordinal)..];
        rule = rule[..rule.IndexOf("OwnerUserIdProperty", StringComparison.Ordinal)];

        Assert.DoesNotContain("IgnoreQueryFilters()", rule);
        Assert.Contains("IncludingUnmoderated()", rule);
    }

    [Fact]
    public void The_public_branch_still_tests_approval_and_the_publication_window()
    {
        var source = RepositoryRoot.ReadFile(
            "Infastrucre/Presitance/Configurations/ModerationQueryExtensions.cs");

        var rule = source[source.IndexOf("VisibleToViewer<TEntity>", StringComparison.Ordinal)..];
        rule = rule[..rule.IndexOf("OwnerUserIdProperty", StringComparison.Ordinal)];

        Assert.Contains("ModerationStatus == ModerationStatus.Approved", rule);
        Assert.Contains("x.ExpireAt == null || x.ExpireAt > DateTime.UtcNow", rule);
    }

    [Fact]
    public void An_anonymous_caller_leaves_the_public_query_untouched()
    {
        var source = RepositoryRoot.ReadFile(
            "Infastrucre/Presitance/Configurations/ModerationQueryExtensions.cs");

        var rule = source[source.IndexOf("VisibleToViewer<TEntity>", StringComparison.Ordinal)..];

        Assert.Contains("if (string.IsNullOrEmpty(viewerUserId))", rule);
        Assert.Contains("return query;", rule);
    }

    [Fact]
    public void The_lost_and_found_view_counter_cannot_fail_the_owners_read()
    {
        var source = RepositoryRoot.ReadFile("Core/Services/LostFoundService.cs");

        var details = source[source.IndexOf(
            "public async Task<LostFoundPostDetailsDto> GetByIdAsync", StringComparison.Ordinal)..];
        details = details[..details.IndexOf("MarkAsReturnedAsync", StringComparison.Ordinal)];

        Assert.Contains("RecordViewAsync", details);
        Assert.Contains("catch (NotFoundException)", details);
    }

    [Fact]
    public void The_shared_view_filter_tolerates_a_listing_it_cannot_resolve_publicly()
    {
        var source = RepositoryRoot.ReadFile("MarkatPlace/Filters/ListingInteractionFilter.cs");

        Assert.Contains("catch (NotFoundException)", source);
    }

    [Fact]
    public void The_viewer_identity_comes_only_from_the_authenticated_principal()
    {
        var context = RepositoryRoot.ReadFile("Infastrucre/Presitance/Data/AppDbContext.cs");

        Assert.Contains("public string? ViewerUserId => _caller?.UserId;", context);

        var caller = RepositoryRoot.ReadFile("Infastrucre/Presitance/Services/AdminActionContext.cs");
        Assert.Contains("ClaimTypes.NameIdentifier", caller);
        foreach (var untrusted in new[] { "Request.Query", "Request.Form", "RouteValues", "Request.Headers" })
            Assert.DoesNotContain($"UserId => _accessor.HttpContext?.{untrusted}", caller);
    }

    private static IEnumerable<Type> ModeratedListingTypes() =>
        typeof(IModeratedListing).Assembly
            .GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false }
                        && typeof(IModeratedListing).IsAssignableFrom(type));
}
