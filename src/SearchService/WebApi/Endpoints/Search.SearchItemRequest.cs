namespace WebApi.Endpoints;

public class SearchItemRequest
{
    public const string Route = "/api/search";

    public Query Query { get; set; }

}

public class Query
{
    public string SearchTerm { get; set; }

    public int PageSize { get; set; } = 25;

    public int PageNumber { get; set; } = 1;

    public string Seller { get; set; }

    public string Winner { get; set; }

    public string OrderBy { get; set; }

    public string FilterBy { get; set; }
}