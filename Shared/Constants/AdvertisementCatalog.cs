using Shared.Enums;

namespace Shared.Constants;

public static class AdvertisementCatalog
{
    public static readonly IReadOnlyDictionary<ListingType, string> ListingTypeNames =
        CarCatalog.ListingTypeNames;

    public static readonly IReadOnlyDictionary<PostType, string> PostTypeNames =
        new Dictionary<PostType, string>
        {
            [PostType.Lost] = "ضايع مني",
            [PostType.Found] = "لقيت"
        };

    public const string EgyptianPhonePattern = @"^01[0-2,5]{1}[0-9]{8}$";

    public const string EgyptianPhoneMessage = "من فضلك اكتب رقم موبايل مصري صحيح (مثال: 01012345678).";

    public const int MinManufacturingYear = CarCatalog.MinManufacturingYear;
}
