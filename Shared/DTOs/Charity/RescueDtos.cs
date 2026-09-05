using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.Charity;

public class CreateRescueRequest : CreateCharityRequestBase
{
    public string RescuerName { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string Details { get; set; } = default!;
}

public class UpdateRescueRequest : CreateRescueRequest, ICharityGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class RescueDetailsDto : CharityDetailsDtoBase
{
    public string RescuerName { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Details { get; set; } = default!;

    public override ListingModuleType StatsListingType => ListingModuleType.Rescue;
}

public class RescueListItemDto : CharityDetailsDtoBase
{
    public string RescuerName { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Details { get; set; } = default!;

    public override ListingModuleType StatsListingType => ListingModuleType.Rescue;
}

public class RescueFilterParams : CharityFilterParamsBase
{
}
