using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Persistence.Data;
using Persistence.Repositories;
using Services.AdForms;
using Services.Lookups;
using Shared.Constants;
using Shared.DTOs.Lookups.Forms;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Settings;
using Xunit;

namespace MarkatPlace.Tests;

public class CreateAdNavigationTests
{
    private static AppDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    private static CategorySelectionResolver Resolver(AppDbContext context) =>
        new(new LookupRepository(context));

    private static AdFormBreadcrumbBuilder Breadcrumbs(AppSettings? settings = null) =>
        new(Options.Create(settings ?? new AppSettings()));

    private static async Task<IReadOnlyList<AdFormBreadcrumbItemDto>> BuildAsync(
        int categoryId, int? subCategoryId, AppSettings? settings = null)
    {
        using var context = NewContext();
        var selection = await Resolver(context).ResolveAsync(categoryId, subCategoryId);

        return Breadcrumbs(settings).Build(
            selection.Category, selection.SubCategory, selection.RequiresSubCategory);
    }

    public static TheoryData<int, int> EverySubCategory()
    {
        using var context = NewContext();
        var data = new TheoryData<int, int>();

        foreach (var sub in context.SubCategories.AsNoTracking().ToList())
            data.Add(sub.CategoryId, sub.Id);

        return data;
    }

    public static TheoryData<int> EveryCategory()
    {
        using var context = NewContext();
        var data = new TheoryData<int>();

        foreach (var category in context.Categories.AsNoTracking().ToList())
            data.Add(category.Id);

        return data;
    }

    [Fact]
    public async Task The_seeded_reference_data_is_visible_to_these_tests()
    {
        using var context = NewContext();

        Assert.NotEmpty(await context.Categories.ToListAsync());
        Assert.NotEmpty(await context.SubCategories.ToListAsync());
    }

    [Theory]
    [MemberData(nameof(EverySubCategory))]
    public async Task Every_category_and_sub_category_produces_a_four_step_Arabic_trail(
        int categoryId, int subCategoryId)
    {
        using var context = NewContext();

        var category = await context.Categories.SingleAsync(c => c.Id == categoryId);
        var subCategory = await context.SubCategories.SingleAsync(s => s.Id == subCategoryId);

        var trail = await BuildAsync(categoryId, subCategoryId);

        Assert.Equal(
            [
                AdFormBreadcrumbLevels.Home,
                AdFormBreadcrumbLevels.Category,
                AdFormBreadcrumbLevels.SubCategory,
                AdFormBreadcrumbLevels.Form
            ],
            trail.Select(step => step.Level));

        Assert.Equal(AdFormBreadcrumbBuilder.HomeNameAr, trail[0].NameAr);
        Assert.Equal(category.Id, trail[1].Id);
        Assert.Equal(category.NameAr, trail[1].NameAr);
        Assert.Equal(subCategory.Id, trail[2].Id);
        Assert.Equal(subCategory.NameAr, trail[2].NameAr);
        Assert.Equal(AdFormBreadcrumbBuilder.CreateAdNameAr, trail[3].NameAr);

        foreach (var step in trail)
        {
            Assert.True(
                ArabicText.IsArabic(step.NameAr),
                $"Breadcrumb step {step.Level} is not labelled in Arabic: {step.NameAr}");
        }
    }

    [Theory]
    [MemberData(nameof(EverySubCategory))]
    public async Task Every_trail_links_back_through_the_create_product_route(
        int categoryId, int subCategoryId)
    {
        var trail = await BuildAsync(categoryId, subCategoryId);

        Assert.Equal(FrontendRoutes.DefaultHomePath, trail[0].Path);
        Assert.Equal($"/create-product/{categoryId}", trail[1].Path);
        Assert.Equal($"/create-product/{categoryId}/{subCategoryId}", trail[2].Path);
        Assert.Equal($"/create-product/{categoryId}/{subCategoryId}", trail[3].Path);
    }

    [Theory]
    [MemberData(nameof(EverySubCategory))]
    public async Task Only_the_last_step_of_a_trail_is_the_current_one(int categoryId, int subCategoryId)
    {
        var trail = await BuildAsync(categoryId, subCategoryId);

        Assert.True(trail[^1].IsCurrent);
        Assert.All(trail.Take(trail.Count - 1), step => Assert.False(step.IsCurrent));
    }

    [Theory]
    [MemberData(nameof(EveryCategory))]
    public async Task A_category_without_a_sub_category_stops_at_the_choose_step(int categoryId)
    {
        using var context = NewContext();
        var selection = await Resolver(context).ResolveAsync(categoryId, null);

        if (!selection.RequiresSubCategory)
            return;

        var trail = Breadcrumbs().Build(selection.Category, null, requiresSubCategory: true);

        Assert.Equal(
            [AdFormBreadcrumbLevels.Home, AdFormBreadcrumbLevels.Category, AdFormBreadcrumbLevels.Form],
            trail.Select(step => step.Level));

        Assert.Equal(AdFormBreadcrumbBuilder.ChooseSubCategoryNameAr, trail[^1].NameAr);
        Assert.Equal($"/create-product/{categoryId}", trail[^1].Path);
        Assert.True(trail[^1].IsCurrent);
    }

    [Fact]
    public async Task A_deep_link_to_the_workshops_form_reads_as_the_workshops_trail()
    {
        var trail = await BuildAsync(
            (int)CategoryType.WorkshopsAndCraftsmen, (int)SubCategoryType.Workshops);

        Assert.Equal(
            ["الرئيسية", "الورش والحرفيين", "الورش", "إنشاء إعلان"],
            trail.Select(step => step.NameAr));
    }

    [Fact]
    public async Task An_unknown_category_is_refused_in_Arabic_and_never_produces_a_trail()
    {
        using var context = NewContext();

        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => Resolver(context).ResolveAsync(9999, null));

        Assert.True(ArabicText.IsArabic(exception.Message));
    }

    [Fact]
    public async Task An_unknown_sub_category_is_refused_in_Arabic_and_never_produces_a_trail()
    {
        using var context = NewContext();

        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => Resolver(context).ResolveAsync((int)CategoryType.WorkshopsAndCraftsmen, 9999));

        Assert.True(ArabicText.IsArabic(exception.Message));
    }

    [Fact]
    public async Task A_sub_category_belonging_to_another_category_is_refused_in_Arabic()
    {
        using var context = NewContext();

        var exception = await Assert.ThrowsAsync<BadRequestException>(
            () => Resolver(context).ResolveAsync(
                (int)CategoryType.WorkshopsAndCraftsmen, (int)SubCategoryType.Private));

        Assert.True(ArabicText.IsArabic(exception.Message));
    }

    [Fact]
    public async Task The_create_advertisement_path_follows_configuration()
    {
        var settings = new AppSettings { CreateAdPath = "add-listing/", HomePath = "home" };

        var trail = await BuildAsync(
            (int)CategoryType.WorkshopsAndCraftsmen, (int)SubCategoryType.Workshops, settings);

        Assert.Equal("/home", trail[0].Path);
        Assert.Equal("/add-listing/2", trail[1].Path);
        Assert.Equal("/add-listing/2/5", trail[2].Path);
    }

    [Fact]
    public void The_documented_upload_limits_repeat_the_platform_constants()
    {
        var limits = CreateAdFormService.UploadLimits();

        Assert.Equal(FileUploadConstants.MaxRequestBodySizeMegabytes, limits.MaxRequestSizeMb);
        Assert.Equal(ImageConstants.MaxImagesPerItem, limits.MaxImages);
        Assert.Equal(5, limits.MaxImageSizeMb);
        Assert.Equal(50, limits.MaxVideoSizeMb);
        Assert.Equal(10, limits.MaxDocumentSizeMb);
        Assert.True(ArabicText.IsArabic(limits.TooLargeMessage));
        Assert.Contains(
            FileUploadConstants.MaxRequestBodySizeMegabytes.ToString(), limits.TooLargeMessage);
    }

    [Fact]
    public void Every_published_create_ad_example_carries_its_own_trail()
    {
        Assert.NotEmpty(CreateAdFormExamples.All);

        foreach (var example in CreateAdFormExamples.All)
        {
            Assert.Equal(4, example.Form.Breadcrumb.Count);
            Assert.Equal(example.Form.Category.Id, example.Form.Breadcrumb[1].Id);
            Assert.Equal(example.Form.SubCategory!.Id, example.Form.Breadcrumb[2].Id);
            Assert.Equal(
                FileUploadConstants.MaxRequestBodySizeMegabytes,
                example.Form.Upload.MaxRequestSizeMb);
        }
    }
}
