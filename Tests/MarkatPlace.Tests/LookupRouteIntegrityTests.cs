using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Services.AdForms;
using Services.ReadConfigs;
using Shared.Constants;
using Shared.DTOs.Lookups.Read;
using Shared.Enums;
using Xunit;

namespace MarkatPlace.Tests;

public class LookupRouteIntegrityTests
{
    private static IReadOnlyList<(int CategoryId, int SubCategoryId)> SeededSelections()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);
        context.Database.EnsureCreated();

        return context.SubCategories.AsNoTracking()
            .OrderBy(sub => sub.Id)
            .Select(sub => new { sub.CategoryId, sub.Id })
            .AsEnumerable()
            .Select(sub => (sub.CategoryId, sub.Id))
            .ToList();
    }

    public static TheoryData<int, int> EverySeededSubCategory()
    {
        var data = new TheoryData<int, int>();

        foreach (var (categoryId, subCategoryId) in SeededSelections())
            data.Add(categoryId, subCategoryId);

        return data;
    }

    [Fact]
    public void The_route_index_sees_real_routes_and_refuses_invented_ones()
    {
        Assert.True(ApiRouteIndex.Routes.Count > 500);

        Assert.True(ApiRouteIndex.Exists("GET", "/api/lookups/create-ad-form/{categoryId}/{subCategoryId}"));
        Assert.True(ApiRouteIndex.Exists("GET", "/api/lookups/create-ad-form/{categoryId}"));
        Assert.True(ApiRouteIndex.Exists("GET", "/api/lookups/advertisers/{categoryId}/{subCategoryId}"));
        Assert.False(ApiRouteIndex.Exists("GET", "/api/lookups/advertisers/{categoryId}"));
        Assert.True(ApiRouteIndex.Exists("POST", "/api/blood-requests"));

        Assert.False(ApiRouteIndex.Exists("GET", "/api/no-such-module"));
        Assert.False(ApiRouteIndex.Exists("POST", "/api/lookups/categories"));
        Assert.False(ApiRouteIndex.Exists("DELETE", "/api/lookups/categories"));
    }

    [Theory]
    [MemberData(nameof(EverySeededSubCategory))]
    public void Every_seeded_sub_category_has_a_create_form_and_a_read_config(
        int categoryId, int subCategoryId)
    {
        Assert.NotNull(AdFormSchemaCatalog.GetSchema(categoryId, subCategoryId));
        Assert.NotNull(ReadConfigCatalog.GetSchema(categoryId, subCategoryId));
    }

    [Theory]
    [MemberData(nameof(EverySeededSubCategory))]
    public void The_advertised_submit_endpoint_is_a_real_POST_route(int categoryId, int subCategoryId)
    {
        var schema = AdFormSchemaCatalog.GetSchema(categoryId, subCategoryId)!;

        Assert.True(ApiRouteIndex.Exists("POST", schema.Submit.Endpoint),
            $"create-ad-form/{categoryId}/{subCategoryId} submits to POST {schema.Submit.Endpoint}, " +
            "which no controller serves.");
    }

    [Theory]
    [MemberData(nameof(EverySeededSubCategory))]
    public void Every_advertised_read_operation_is_a_real_GET_route(int categoryId, int subCategoryId)
    {
        var schema = ReadConfigCatalog.GetSchema(categoryId, subCategoryId)!;

        var missing = schema.Operations
            .Where(operation => !ApiRouteIndex.Exists("GET", operation.Endpoint))
            .Select(operation => $"{operation.Key} → GET {operation.Endpoint}")
            .ToList();

        Assert.True(missing.Count == 0,
            $"read-config/{categoryId}/{subCategoryId} advertises routes no controller serves: " +
            string.Join("; ", missing));
    }

    [Theory]
    [MemberData(nameof(EverySeededSubCategory))]
    public void The_list_and_submit_endpoints_name_the_same_module(int categoryId, int subCategoryId)
    {
        var form = AdFormSchemaCatalog.GetSchema(categoryId, subCategoryId)!;
        var read = ReadConfigCatalog.GetSchema(categoryId, subCategoryId)!;

        var list = read.Find(ReadOperationKeys.List);

        Assert.NotNull(list);
        Assert.Equal(form.Submit.Endpoint, list!.Endpoint);
    }

    [Theory]
    [MemberData(nameof(EverySeededSubCategory))]
    public void The_seeded_parent_category_matches_the_module_catalog(int categoryId, int subCategoryId)
    {
        Assert.True(Enum.IsDefined(typeof(SubCategoryType), subCategoryId),
            $"Seeded sub-category {subCategoryId} has no SubCategoryType member.");

        Assert.Equal((CategoryType)categoryId,
            ListingModuleCatalog.CategoryOf((SubCategoryType)subCategoryId));
    }

    [Theory]
    [MemberData(nameof(EverySeededSubCategory))]
    public void The_module_route_in_the_notification_catalog_is_the_list_route(
        int categoryId, int subCategoryId)
    {
        if (ListingModuleCatalog.ModuleOf(subCategoryId) is not { } module ||
            ListingModuleCatalog.IsIsolated(module))
            return;

        var read = ReadConfigCatalog.GetSchema(categoryId, subCategoryId)!;
        var list = read.Find(ReadOperationKeys.List)!;

        Assert.Equal(list.Endpoint, "/api/" + ListingModuleCatalog.RouteOf(module));
    }
}
