namespace Shared.Responses;

public class PaginatedResult<T>
{
    public PaginatedResult() { }

    public PaginatedResult(IReadOnlyList<T> items, int totalCount, int pageIndex, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => PageIndex > 1;
    public bool HasNext => PageIndex < TotalPages;
}
