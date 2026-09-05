using System.Text.RegularExpressions;
using Shared.Constants;
using Xunit;

namespace MarkatPlace.Tests;

public class EgyptianInputTests
{
    private static readonly Regex Phone =
        new(AdvertisementCatalog.EgyptianPhonePattern, RegexOptions.Compiled);

    public static TheoryData<string> RealEgyptianMobileNumbers() => new()
    {
        "01012345678",
        "01112345678",
        "01212345678",
        "01512345678",
        "01000000000",
        "01599999999",
    };

    [Theory]
    [MemberData(nameof(RealEgyptianMobileNumbers))]
    public void Every_real_Egyptian_mobile_prefix_is_accepted(string number)
    {
        Assert.True(Phone.IsMatch(number),
            $"{number} is a valid Egyptian mobile number and must not be rejected.");
    }

    [Theory]
    [InlineData("0131234567")]
    [InlineData("01712345678")]
    [InlineData("0101234567")]
    [InlineData("010123456789")]
    [InlineData("1012345678")]
    [InlineData("")]
    [InlineData("not a phone")]
    public void Numbers_that_cannot_be_Egyptian_mobiles_are_rejected(string number)
    {
        Assert.DoesNotMatch(Phone, number);
    }

    [Fact]
    public void The_phone_error_message_is_Egyptian_Arabic_and_shows_an_example()
    {
        Assert.True(ArabicText.IsArabic(AdvertisementCatalog.EgyptianPhoneMessage));
        Assert.Contains("01012345678", AdvertisementCatalog.EgyptianPhoneMessage);
    }

    [Theory]
    [InlineData("الفيوم")]
    [InlineData("سنورس")]
    [InlineData("طامية")]
    [InlineData("يوسف الصديق")]
    [InlineData("اطسا")]
    [InlineData("ابشواي")]
    [InlineData("الفيوم الجديدة")]
    public void Every_Fayoum_center_is_accepted(string center)
    {
        Assert.True(LocationConstants.IsValidCenter(center));
    }

    [Theory]
    [InlineData("القاهرة")]
    [InlineData("الجيزة")]
    [InlineData("Fayoum")]
    [InlineData(" الفيوم ")]
    [InlineData("")]
    [InlineData(null)]
    public void A_place_outside_Fayoum_is_not_a_center(string? center)
    {
        Assert.False(LocationConstants.IsValidCenter(center));
    }

    [Fact]
    public void The_governorate_is_fixed_to_Fayoum()
    {
        Assert.True(LocationConstants.IsValidGovernorate("الفيوم"));
        Assert.False(LocationConstants.IsValidGovernorate("القاهرة"));
    }

    [Fact]
    public void Center_order_is_stable_because_ids_are_derived_from_position()
    {
        Assert.Equal(
            new[] { "الفيوم", "سنورس", "طامية", "يوسف الصديق", "اطسا", "ابشواي", "الفيوم الجديدة" },
            LocationConstants.Centers.ToArray());
    }
}
