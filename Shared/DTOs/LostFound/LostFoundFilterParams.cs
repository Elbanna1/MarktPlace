using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.LostFound;

public class LostFoundFilterParams : PaginationParams
{
    public PostType? PostType { get; set; }

    public string? Search { get; set; }

    public string? City { get; set; }

    public DateOnly? Date { get; set; }

    public DateOnly? LostDate
    {
        get => Date;
        set => Date = value ?? Date;
    }

    public DateOnly? FoundDate
    {
        get => Date;
        set => Date = value ?? Date;
    }
}
