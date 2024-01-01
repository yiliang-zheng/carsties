using MongoDB.Entities;
using WebApi.Endpoints;
using WebApi.Models;
using Search = MongoDB.Entities.Search;

namespace WebApi.Repositories;

public class SearchRepository : ISearchRepository
{
    public async Task<SearchResult> SearchItems(Query query, CancellationToken token)
    {
        var result = DB.PagedSearch<Item, Item>();

        if (!string.IsNullOrEmpty(query.SearchTerm))
        {
            result = result
                .Match(Search.Full, query.SearchTerm)
                .SortByTextScore();
        }

        result = query.OrderBy switch
        {
            "make" => result
                .Sort(p => p.Ascending(i => i.Make))
                .Sort(p => p.Ascending(i => i.Make)),
            "new" => result.Sort(p => p.Descending(i => i.CreatedAt)),
            _ => result.Sort(p => p.Descending(i => i.AuctionEnd))
        };

        result = query.FilterBy switch
        {
            "finished" => result.Match(p => p.AuctionEnd < DateTime.UtcNow),
            "endingSoon" => result.Match(p => p.AuctionEnd < DateTime.UtcNow.AddHours(6)),
            _ => result.Match(p => p.AuctionEnd > DateTime.UtcNow)
        };

        if (!string.IsNullOrEmpty(query.Seller))
        {
            result = result.Match(p => p.Seller.Equals(query.Seller));
        }

        if (!string.IsNullOrEmpty(query.Winner))
        {
            result = result.Match(p => p.Winner.Equals(query.Winner));
        }

        result.PageNumber(query.PageNumber);
        result.PageSize(query.PageSize);


        var pageResult = new SearchResult(await result.ExecuteAsync(token));
        return pageResult;
    }
}