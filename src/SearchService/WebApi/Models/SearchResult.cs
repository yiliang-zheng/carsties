namespace WebApi.Models;

public class SearchResult
{
    public SearchResult((IReadOnlyList<Item> Results, long TotalCount, int PageCount) tuple)
    {
        Results = tuple.Results;
        TotalCount = tuple.TotalCount;
        PageCount = tuple.PageCount;
    }
    public IReadOnlyList<Item> Results { get; set; }

    public long TotalCount { get; set; }

    public int PageCount { get; set; }
}
