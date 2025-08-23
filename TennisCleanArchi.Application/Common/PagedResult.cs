namespace TennisCleanArchi.Application.Common;

public class PagedResult<T>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int Total { get; init; }
    public List<T> Items { get; init; } = new();

    public PagedResult(int pageNumber, int pageSize, int total, List<T> items)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Total = total;
        Items = items;
    }
}
