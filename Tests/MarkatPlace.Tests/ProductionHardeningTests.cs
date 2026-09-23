using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using FluentValidation;
using Services.Validation;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Common;
using Shared.Responses;
using Xunit;

namespace MarkatPlace.Tests;

/// <summary>
/// Covers the production-hardening pass: Arabic-safe JSON, the pagination and feature-selection
/// ceilings, deterministic car ordering, and the removal of seller contact details from the public
/// listing feeds.
/// </summary>
public class ProductionHardeningTests
{
    private sealed class Payload
    {
        public string Title { get; set; } = default!;

        public string? Missing { get; set; }

        public int? Count { get; set; }
    }

    [Fact]
    public void Arabic_is_serialized_as_Arabic_and_not_as_escape_sequences()
    {
        var json = JsonSerializer.Serialize(
            new Payload { Title = "سوق الفيوم" }, ApiJson.SerializerOptions);

        Assert.Contains("سوق الفيوم", json, StringComparison.Ordinal);
        Assert.DoesNotContain("\\u", json, StringComparison.Ordinal);
    }

    [Fact]
    public void English_and_digits_survive_the_encoder_unchanged()
    {
        var json = JsonSerializer.Serialize(
            new Payload { Title = "Toyota Corolla 2020", Count = 7 }, ApiJson.SerializerOptions);

        Assert.Contains("\"title\":\"Toyota Corolla 2020\"", json, StringComparison.Ordinal);
        Assert.Contains("\"count\":7", json, StringComparison.Ordinal);
    }

    [Fact]
    public void The_encoder_still_escapes_the_characters_that_make_HTML_injection_possible()
    {
        var json = JsonSerializer.Serialize(
            new Payload { Title = "<script>alert('x')&</script>" }, ApiJson.SerializerOptions);

        Assert.DoesNotContain("<script>", json, StringComparison.Ordinal);
        Assert.Contains("\\u003C", json, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("\\u0026", json, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Null_properties_are_left_out_of_a_payload()
    {
        var json = JsonSerializer.Serialize(
            new Payload { Title = "عنوان" }, ApiJson.SerializerOptions);

        Assert.DoesNotContain("missing", json, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("count", json, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void The_response_envelope_keeps_its_null_members_so_the_contract_is_unchanged()
    {
        var success = JsonSerializer.Serialize(
            ApiResponse<Payload>.Ok(null, "تم"), ApiJson.SerializerOptions);

        Assert.Contains("\"data\":null", success, StringComparison.Ordinal);
        Assert.Contains("\"errors\":null", success, StringComparison.Ordinal);

        var failure = JsonSerializer.Serialize(ApiResponse.Fail("خطأ"), ApiJson.SerializerOptions);

        Assert.Contains("\"errors\":null", failure, StringComparison.Ordinal);
        Assert.Contains("\"success\":false", failure, StringComparison.Ordinal);
    }

    [Fact]
    public void A_page_index_below_one_is_still_clamped_rather_than_refused()
    {
        var paging = new PaginationParams { PageIndex = 0 };

        Assert.Equal(1, paging.PageIndex);
        Assert.False(paging.IsPageIndexOutOfRange);
    }

    [Fact]
    public void The_last_allowed_page_is_accepted_and_the_one_after_it_is_not()
    {
        Assert.False(new PaginationParams { PageIndex = PaginationParams.MaxPageIndex }
            .IsPageIndexOutOfRange);

        Assert.True(new PaginationParams { PageIndex = PaginationParams.MaxPageIndex + 1 }
            .IsPageIndexOutOfRange);
    }

    [Fact]
    public void The_largest_allowed_page_cannot_overflow_the_skip_calculation()
    {
        var skip = (long)(PaginationParams.MaxPageIndex - 1) * PaginationParams.MaxPageSize;

        Assert.InRange(skip, 0, int.MaxValue);
    }

    [Fact]
    public void The_shared_paging_helper_clamps_a_skip_that_would_overflow()
    {
        var helper = RepositoryRoot.ReadFile(
            "Infastrucre/Presitance/Repositories/PagedListingQuery.cs");

        Assert.Contains("Math.Clamp((long)(pageIndex - 1) * pageSize", helper, StringComparison.Ordinal);
    }

    [Fact]
    public void An_out_of_range_page_index_is_refused_by_the_shared_validation_filter()
    {
        var filter = RepositoryRoot.ReadFile("MarkatPlace/Filters/ValidationFilter.cs");

        Assert.Contains("is PaginationParams { IsPageIndexOutOfRange: true }", filter, StringComparison.Ordinal);
        Assert.Contains("UserMessages.Errors.PageIndexOutOfRange", filter, StringComparison.Ordinal);
        Assert.Contains("Status422UnprocessableEntity", filter, StringComparison.Ordinal);
    }

    [Fact]
    public void A_feature_filter_at_the_ceiling_is_accepted_and_one_above_it_is_not()
    {
        var validator = new AdvertisementFilterParamsValidator();

        var atCeiling = new AdvertisementFilterParams
        {
            FeatureIds = Enumerable.Range(1, AdvertisementConstants.MaxFeatureSelection).ToList()
        };

        var overCeiling = new AdvertisementFilterParams
        {
            FeatureIds = Enumerable.Range(1, AdvertisementConstants.MaxFeatureSelection + 1).ToList()
        };

        Assert.True(validator.Validate(atCeiling).IsValid);
        Assert.False(validator.Validate(overCeiling).IsValid);
    }

    [Fact]
    public void A_filter_with_no_features_at_all_is_still_valid()
    {
        var validator = new AdvertisementFilterParamsValidator();

        Assert.True(validator.Validate(new AdvertisementFilterParams()).IsValid);
        Assert.True(validator.Validate(new AdvertisementFilterParams { FeatureIds = [] }).IsValid);
    }

    [Fact]
    public void The_write_paths_and_the_filter_share_one_feature_ceiling()
    {
        foreach (var file in new[]
        {
            "Core/Services/Validation/CreateAdvertisementRequestValidator.cs",
            "Core/Services/Validation/UpdateAdvertisementRequestValidator.cs",
            "Core/Services/Validation/AdvertisementFilterParamsValidator.cs"
        })
        {
            Assert.Contains("AdvertisementConstants.MaxFeatureSelection",
                RepositoryRoot.ReadFile(file), StringComparison.Ordinal);
        }
    }

    [Fact]
    public void Every_car_sort_ends_on_the_identifier_so_pages_cannot_interleave()
    {
        var source = RepositoryRoot.ReadFile(
            "Infastrucre/Presitance/Repositories/AdvertisementRepository.cs");

        var sorting = source[source.IndexOf("ApplySorting(IQueryable<Advertisement> query",
            StringComparison.Ordinal)..];

        sorting = sorting[..sorting.IndexOf("};", StringComparison.Ordinal)];

        var branches = Regex.Matches(sorting, @"=>\s*query[\s\S]*?(?=,\s*\r?\n\s*\r?\n|$)");

        Assert.Equal(5, branches.Count);

        foreach (Match branch in branches)
        {
            Assert.True(
                branch.Value.Contains("ThenBy(a => a.Id)", StringComparison.Ordinal) ||
                branch.Value.Contains("ThenByDescending(a => a.Id)", StringComparison.Ordinal),
                $"A car sort has no identifier tie-breaker:{Environment.NewLine}{branch.Value}");
        }
    }

    [Fact]
    public void The_car_feed_indexes_carry_the_same_tie_breaker_as_the_sort()
    {
        var configuration = RepositoryRoot.ReadFile(
            "Infastrucre/Presitance/Configurations/AdvertisementConfiguration.cs");

        var feedIndexes = Regex.Matches(configuration, @"HasIndex\(\s*\r?\n?\s*a => new \{ a\.ModerationStatus[^}]*\}");

        Assert.Equal(4, feedIndexes.Count);

        foreach (Match index in feedIndexes)
            Assert.Contains("a.Id", index.Value, StringComparison.Ordinal);
    }

    public static IEnumerable<object[]> PublicListDtos() =>
        typeof(AdvertisementListItemDto).Assembly
            .GetTypes()
            .Where(type => type is { IsClass: true, IsPublic: true })
            .Where(type => type.Name.Contains("ListItem", StringComparison.Ordinal) ||
                           type.Name.EndsWith("ListDto", StringComparison.Ordinal))
            .Where(type => !type.Name.StartsWith("Admin", StringComparison.Ordinal))
            .Select(type => new object[] { type });

    [Theory]
    [MemberData(nameof(PublicListDtos))]
    public void No_public_listing_row_carries_the_advertisers_phone_or_WhatsApp(Type listDto)
    {
        var leaked = listDto
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(property => property.Name)
            .Where(name =>
                name.Contains("Phone", StringComparison.OrdinalIgnoreCase) ||
                name.Contains("WhatsApp", StringComparison.OrdinalIgnoreCase))
            .ToList();

        Assert.True(leaked.Count == 0,
            $"{listDto.Name} publishes {string.Join(", ", leaked)} on a public listing feed. " +
            "Contact details belong on the details payload only.");
    }

    [Fact]
    public void The_details_payloads_still_carry_the_contact_details_the_listing_page_needs()
    {
        foreach (var name in new[] { "SheepGoatDetailsDto", "LandDetailsDto", "WorkshopDetailsDto" })
        {
            var type = typeof(AdvertisementListItemDto).Assembly
                .GetTypes()
                .Single(candidate => candidate.Name == name);

            Assert.Contains(
                type.GetProperties(BindingFlags.Public | BindingFlags.Instance),
                property => property.Name.Contains("Phone", StringComparison.OrdinalIgnoreCase));
        }
    }

    [Fact]
    public void The_cars_feed_publishes_an_owner_without_a_phone_number_while_the_details_owner_keeps_one()
    {
        Assert.DoesNotContain(
            typeof(ListingOwnerDto).GetProperties(BindingFlags.Public | BindingFlags.Instance),
            property => property.Name == nameof(OwnerDto.PhoneNumber));

        Assert.Equal(typeof(ListingOwnerDto), typeof(AdvertisementListItemDto)
            .GetProperty(nameof(AdvertisementListItemDto.Owner))!.PropertyType);

        Assert.Contains(
            typeof(OwnerDto).GetProperties(BindingFlags.Public | BindingFlags.Instance),
            property => property.Name == nameof(OwnerDto.PhoneNumber));
    }

    [Fact]
    public void A_charity_card_carries_no_phone_but_a_charity_details_payload_does()
    {
        var assembly = typeof(AdvertisementListItemDto).Assembly;

        var listBase = assembly.GetTypes().Single(type => type.Name == "CharityListItemDtoBase");
        var detailsBase = assembly.GetTypes().Single(type => type.Name == "CharityDetailsDtoBase");

        Assert.DoesNotContain(
            listBase.GetProperties(BindingFlags.Public | BindingFlags.Instance),
            property => property.Name == "Phone");

        Assert.Contains(
            detailsBase.GetProperties(BindingFlags.Public | BindingFlags.Instance),
            property => property.Name == "Phone");
    }

    [Fact]
    public void No_public_listing_row_carries_a_video_that_only_the_details_page_can_play()
    {
        var offenders = PublicListDtos()
            .Select(row => (Type)row[0])
            .Where(type => type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Any(property => property.Name is "Video" or "VideoUrl"))
            .Select(type => type.Name)
            .ToList();

        Assert.True(offenders.Count == 0,
            "A listing card never plays a video, yet these rows still carry one: " +
            string.Join(", ", offenders));
    }
}
