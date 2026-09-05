using Shared.Enums;

namespace Shared.DTOs.Charity;

public class CreateBloodRequestRequest : CreateCharityRequestBase
{
    public string RequesterName { get; set; } = default!;

    public BloodGroup? BloodGroup { get; set; }

    public string Center { get; set; } = default!;

    public string HospitalName { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string Details { get; set; } = default!;
}

public class UpdateBloodRequestRequest : CreateBloodRequestRequest, ICharityGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class BloodRequestDetailsDto : CharityDetailsDtoBase
{
    public string RequesterName { get; set; } = default!;

    public BloodGroup BloodGroup { get; set; }

    public string BloodGroupName { get; set; } = default!;

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;
    public string HospitalName { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Details { get; set; } = default!;

    public override ListingModuleType StatsListingType => ListingModuleType.BloodRequest;
}

public class BloodRequestListItemDto : CharityDetailsDtoBase
{
    public string RequesterName { get; set; } = default!;
    public BloodGroup BloodGroup { get; set; }
    public string BloodGroupName { get; set; } = default!;
    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;
    public string HospitalName { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Details { get; set; } = default!;

    public override ListingModuleType StatsListingType => ListingModuleType.BloodRequest;
}

public class BloodRequestFilterParams : CharityFilterParamsBase
{
    public BloodGroup? BloodGroup { get; set; }

    public string? Center { get; set; }
}
