namespace Shared.DTOs.Common;

public class PaginationParams
{
    private const int MaxPageSize = 50;
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
}
