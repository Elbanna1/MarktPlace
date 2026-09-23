namespace Shared.DTOs.Common;

public class PaginationParams
{
    public const int MaxPageSize = 50;

    public const int MaxPageIndex = 10_000;

    private int _pageSize = 10;
    private int _pageIndex = 1;

    public int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 10 : (value > MaxPageSize ? MaxPageSize : value);
    }

    public bool IsPageIndexOutOfRange => _pageIndex > MaxPageIndex;
}
