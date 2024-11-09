namespace WebMVC.Application;

public class PaginatedList<T>
{
    public List<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => Items.Count == PageSize;

    public PaginatedList(List<T> items, int pageNumber, int pageSize)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
