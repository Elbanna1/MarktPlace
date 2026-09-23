using Shared.DTOs.Common;

namespace Shared.DTOs.Lookups;

public class AdvertiserOptionDto
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public int ListingsCount { get; set; }
}

public class AdvertiserLookupParams : PaginationParams
{
    public const int DefaultPageSize = 20;

    public const int MaxSearchLength = 150;

    public AdvertiserLookupParams()
    {
        PageSize = DefaultPageSize;
    }

    public string? Search { get; set; }
}
