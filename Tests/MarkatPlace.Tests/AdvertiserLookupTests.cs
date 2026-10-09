using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Persistence.Listings;
using Persistence.Data;
using Persistence.Repositories;
using Services.Lookups;
using Services.ReadConfigs;
using Shared.Constants;
using Shared.DTOs.Lookups;
using Shared.DTOs.Lookups.Read;
using Shared.Enums;
using Shared.Exceptions;
using Xunit;

namespace MarkatPlace.Tests;

public class AdvertiserLookupTests
{
    private const int AnimalsCategory = 6;

    private const int BirdsSubCategory = (int)SubCategoryType.Birds;

    private static AppDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    private static AdvertiserLookupService Service(AppDbContext context) =>
        new(new CategorySelectionResolver(new LookupRepository(context)),
            new AdvertiserLookupRepository(context));

    private static Bird Listing(string sellerName, ModerationStatus status = ModerationStatus.Approved) =>
        new()
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid().ToString(),
            SellerName = sellerName,
            Title = "عنوان",
            Description = "وصف",
            Address = "العنوان",
            Phone = "01000000000",
            Price = 100,
            Quantity = 1,
            ModerationStatus = status,
            PublishedAt = status == ModerationStatus.Approved ? DateTime.UtcNow : null,
            ExpireAt = status == ModerationStatus.Approved ? DateTime.UtcNow.AddDays(30) : null
        };

    private static async Task SeedBirdsAsync(AppDbContext context, params string[] sellerNames)
    {
        context.Birds.AddRange(sellerNames.Select(name => Listing(name)));
        await context.SaveChangesAsync();
    }

    private static AdvertiserLookupParams Page(int pageIndex = 1, int? pageSize = null, string? search = null)
    {
        var parameters = new AdvertiserLookupParams { PageIndex = pageIndex, Search = search };

        if (pageSize is { } size)
            parameters.PageSize = size;

        return parameters;
    }

    [Fact]
    public async Task No_advertisers_returns_an_empty_page_not_an_error()
    {
        using var context = NewContext();

        var result = await Service(context)
            .GetAdvertisersAsync(AnimalsCategory, BirdsSubCategory, Page());

        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
        Assert.Equal(0, result.TotalPages);
        Assert.Equal(1, result.PageIndex);
        Assert.Equal(AdvertiserLookupParams.DefaultPageSize, result.PageSize);
    }

    [Fact]
    public async Task One_advertiser_returns_one_option()
    {
        using var context = NewContext();
        await SeedBirdsAsync(context, "محمد للطيور");

        var result = await Service(context)
            .GetAdvertisersAsync(AnimalsCategory, BirdsSubCategory, Page());

        var only = Assert.Single(result.Items);
        Assert.Equal("محمد للطيور", only.Name);
        Assert.Equal("محمد للطيور", only.Id);
        Assert.Equal(1, only.ListingsCount);
        Assert.Equal(1, result.TotalCount);
    }

    [Fact]
    public async Task Many_listings_by_one_advertiser_collapse_into_a_single_counted_option()
    {
        using var context = NewContext();
        await SeedBirdsAsync(context, "محمد للطيور", "محمد للطيور", "محمد للطيور", "نور");

        var result = await Service(context)
            .GetAdvertisersAsync(AnimalsCategory, BirdsSubCategory, Page());

        Assert.Equal(2, result.TotalCount);
        Assert.Equal(3, result.Items.Single(x => x.Name == "محمد للطيور").ListingsCount);
        Assert.Equal(1, result.Items.Single(x => x.Name == "نور").ListingsCount);
    }

    [Fact]
    public async Task Exactly_one_page_of_advertisers_reports_a_single_page()
    {
        using var context = NewContext();
        await SeedBirdsAsync(context, Enumerable.Range(0, 20).Select(i => $"بائع {i:D2}").ToArray());

        var result = await Service(context)
            .GetAdvertisersAsync(AnimalsCategory, BirdsSubCategory, Page());

        Assert.Equal(20, result.Items.Count);
        Assert.Equal(20, result.TotalCount);
        Assert.Equal(1, result.TotalPages);
        Assert.False(result.HasNext);
    }

    [Fact]
    public async Task More_advertisers_than_one_page_paginate_in_the_database()
    {
        using var context = NewContext();
        await SeedBirdsAsync(context, Enumerable.Range(0, 55).Select(i => $"بائع {i:D2}").ToArray());

        var service = Service(context);

        var first = await service.GetAdvertisersAsync(AnimalsCategory, BirdsSubCategory, Page());
        var third = await service.GetAdvertisersAsync(AnimalsCategory, BirdsSubCategory, Page(pageIndex: 3));

        Assert.Equal(20, first.Items.Count);
        Assert.Equal(15, third.Items.Count);
        Assert.Equal(55, first.TotalCount);
        Assert.Equal(3, first.TotalPages);
        Assert.True(first.HasNext);
        Assert.False(third.HasNext);
        Assert.Empty(first.Items.Select(x => x.Name).Intersect(third.Items.Select(x => x.Name)));
    }

    [Fact]
    public async Task A_thousand_advertisers_still_return_one_small_page()
    {
        using var context = NewContext();
        await SeedBirdsAsync(context, Enumerable.Range(0, 1000).Select(i => $"بائع {i:D4}").ToArray());

        var result = await Service(context)
            .GetAdvertisersAsync(AnimalsCategory, BirdsSubCategory, Page());

        Assert.Equal(20, result.Items.Count);
        Assert.Equal(1000, result.TotalCount);
        Assert.Equal(50, result.TotalPages);
    }

    [Fact]
    public async Task Options_are_ordered_by_name()
    {
        using var context = NewContext();
        await SeedBirdsAsync(context, "جمال", "احمد", "بسام");

        var result = await Service(context)
            .GetAdvertisersAsync(AnimalsCategory, BirdsSubCategory, Page());

        Assert.Equal(new[] { "احمد", "بسام", "جمال" }, result.Items.Select(x => x.Name).ToArray());
    }

    [Fact]
    public async Task A_page_past_the_end_returns_an_empty_page_with_the_real_total()
    {
        using var context = NewContext();
        await SeedBirdsAsync(context, "احمد", "بسام");

        var result = await Service(context)
            .GetAdvertisersAsync(AnimalsCategory, BirdsSubCategory, Page(pageIndex: 9));

        Assert.Empty(result.Items);
        Assert.Equal(2, result.TotalCount);
    }

    [Fact]
    public async Task Unmoderated_and_deleted_listings_do_not_leak_an_advertiser()
    {
        using var context = NewContext();
        context.Birds.Add(Listing("قيد المراجعة", ModerationStatus.Pending));
        context.Birds.Add(Listing("مرفوض", ModerationStatus.Rejected));

        var deleted = Listing("محذوف");
        deleted.IsDeleted = true;
        context.Birds.Add(deleted);

        context.Birds.Add(Listing("منشور"));
        await context.SaveChangesAsync();

        var result = await Service(context)
            .GetAdvertisersAsync(AnimalsCategory, BirdsSubCategory, Page());

        var only = Assert.Single(result.Items);
        Assert.Equal("منشور", only.Name);
    }

    [Fact]
    public async Task An_expired_listing_does_not_leak_an_advertiser()
    {
        using var context = NewContext();

        var expired = Listing("منتهي");
        expired.ExpireAt = DateTime.UtcNow.AddDays(-1);
        context.Birds.Add(expired);
        context.Birds.Add(Listing("سارى"));
        await context.SaveChangesAsync();

        var result = await Service(context)
            .GetAdvertisersAsync(AnimalsCategory, BirdsSubCategory, Page());

        var only = Assert.Single(result.Items);
        Assert.Equal("سارى", only.Name);
    }

    [Fact]
    public async Task Page_size_is_clamped_to_the_project_maximum()
    {
        using var context = NewContext();
        await SeedBirdsAsync(context, Enumerable.Range(0, 80).Select(i => $"بائع {i:D2}").ToArray());

        var result = await Service(context)
            .GetAdvertisersAsync(AnimalsCategory, BirdsSubCategory, Page(pageSize: 5000));

        Assert.Equal(Shared.DTOs.Common.PaginationParams.MaxPageSize, result.PageSize);
        Assert.Equal(Shared.DTOs.Common.PaginationParams.MaxPageSize, result.Items.Count);
    }

    [Fact]
    public async Task An_unknown_category_is_a_not_found()
    {
        using var context = NewContext();

        await Assert.ThrowsAsync<NotFoundException>(() =>
            Service(context).GetAdvertisersAsync(9999, BirdsSubCategory, Page()));
    }

    [Fact]
    public async Task A_sub_category_from_another_category_is_a_bad_request()
    {
        using var context = NewContext();

        await Assert.ThrowsAsync<BadRequestException>(() =>
            Service(context).GetAdvertisersAsync(
                AnimalsCategory, (int)SubCategoryType.Apartments, Page()));
    }

    [Fact]
    public async Task A_sub_category_whose_listings_have_no_advertiser_filter_is_refused()
    {
        using var context = NewContext();

        var error = await Assert.ThrowsAsync<BadRequestException>(() =>
            Service(context).GetAdvertisersAsync(
                11, (int)SubCategoryType.Apartments, Page()));

        Assert.Contains("المعلن", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void The_catalog_covers_exactly_the_list_filters_that_take_their_options_from_the_advertisers()
    {
        var published = new HashSet<SubCategoryType>();

        foreach (var sub in Enum.GetValues<SubCategoryType>())
        {
            if (ListingModuleCatalog.CategoryOf(sub) is not { } category)
                continue;

            var list = ReadConfigCatalog.GetSchema((int)category, (int)sub)?.Find(ReadOperationKeys.List);

            var fed = list?.QueryParameters?
                .Where(parameter => parameter.OptionsSource == ReadOperationKeys.Advertisers)
                .ToList() ?? [];

            if (fed.Count == 0)
                continue;

            published.Add(sub);

            var parameter = Assert.Single(fed);
            Assert.Equal(AdvertiserLookupCatalog.FilterParameterOf(sub), parameter.Name);
            Assert.Equal(ReadOptionsValues.Name, parameter.OptionsValue);
        }

        Assert.Equal(
            AdvertiserLookupCatalog.SupportedSubCategories.OrderBy(sub => sub),
            published.OrderBy(sub => sub));
    }

    [Fact]
    public void Every_supported_sub_category_is_dispatched_by_the_repository()
    {
        Assert.Equal(
            AdvertiserLookupCatalog.SupportedSubCategories.OrderBy(sub => sub),
            AdvertiserLookupRepository.SupportedSubCategories.OrderBy(sub => sub));
    }

    [Fact]
    public async Task Every_supported_sub_category_answers_over_its_own_table()
    {
        using var context = NewContext();
        var repository = new AdvertiserLookupRepository(context);

        foreach (var sub in AdvertiserLookupCatalog.SupportedSubCategories)
        {
            var (items, totalCount) = await repository.GetPagedAsync(sub, null, 1, 20);

            Assert.Empty(items);
            Assert.Equal(0, totalCount);
        }
    }

    [Theory]
    [InlineData((int)SubCategoryType.Birds, 6, true)]
    [InlineData((int)SubCategoryType.Handmade, 7, true)]
    [InlineData((int)SubCategoryType.Apartments, 11, false)]
    [InlineData((int)SubCategoryType.Furniture, 10, false)]
    [InlineData((int)SubCategoryType.Private, 1, false)]
    public void Read_config_publishes_the_advertisers_operation_only_where_the_filter_exists(
        int subCategoryId, int categoryId, bool expected)
    {
        var schema = ReadConfigCatalog.GetSchema(categoryId, subCategoryId);

        Assert.NotNull(schema);
        Assert.Equal(expected, schema!.Find(ReadOperationKeys.Advertisers) is not null);
    }

    [Fact]
    public void The_published_advertisers_operation_points_at_the_live_route()
    {
        var schema = ReadConfigCatalog.GetSchema(AnimalsCategory, BirdsSubCategory);
        var operation = schema!.Find(ReadOperationKeys.Advertisers);

        Assert.NotNull(operation);
        Assert.Equal("/api/lookups/advertisers/{categoryId}/{subCategoryId}", operation!.Endpoint);
        Assert.True(operation.Paginated);
        Assert.False(operation.RequiresAuthentication);

        Assert.Equal(AnimalsCategory,
            operation.RouteParameters!.Single(p => p.Name == "categoryId").DefaultValue);
        Assert.Equal(BirdsSubCategory,
            operation.RouteParameters!.Single(p => p.Name == "subCategoryId").DefaultValue);
    }

    [Fact]
    public void The_seller_name_filter_points_at_the_advertisers_operation_for_its_options()
    {
        var schema = ReadConfigCatalog.GetSchema(AnimalsCategory, BirdsSubCategory);
        var list = schema!.Find(ReadOperationKeys.List);

        var sellerName = list!.QueryParameters!.Single(p => p.Name == "sellerName");

        Assert.Equal(ReadOperationKeys.Advertisers, sellerName.OptionsSource);
        Assert.Equal(ReadOptionsValues.Name, sellerName.OptionsValue);
    }

    [Fact]
    public void The_lookup_exposes_nothing_but_the_label_and_its_count()
    {
        var properties = typeof(AdvertiserOptionDto)
            .GetProperties()
            .Select(property => property.Name)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(new[] { "Id", "ListingsCount", "Name" }, properties);
    }

    [Fact]
    public void Every_advertiser_filtering_table_carries_the_covering_index()
    {
        var model = SqlServerModel();

        foreach (var sub in AdvertiserLookupCatalog.SupportedSubCategories)
        {
            var module = ListingModuleCatalog.ModuleOf(sub)!.Value;
            var entity = model.FindEntityType(ModerationModuleRegistry.EntityTypes[module])!;

            var index = entity.GetIndexes()
                .SingleOrDefault(candidate =>
                    candidate.GetDatabaseName()?.EndsWith("_Advertisers", StringComparison.Ordinal) == true);

            Assert.True(index is not null, $"{entity.GetTableName()} has no IX_*_Advertisers index.");

            Assert.Equal("ModerationStatus", index!.Properties[0].Name);
            Assert.Equal(2, index.Properties.Count);

            Assert.Equal(
                new[] { "ExpireAt", "IsDeleted" },
                (index.GetIncludeProperties() ?? []).OrderBy(name => name, StringComparer.Ordinal));
        }
    }

    private static Microsoft.EntityFrameworkCore.Metadata.IModel SqlServerModel()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=localhost;Database=ModelOnly;Trusted_Connection=True;")
            .Options;

        using var context = new AppDbContext(options);

        return context.GetService<Microsoft.EntityFrameworkCore.Metadata.IDesignTimeModel>().Model;
    }
}
