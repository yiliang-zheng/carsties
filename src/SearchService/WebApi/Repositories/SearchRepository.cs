using MongoDB.Entities;
using WebApi.Endpoints;
using WebApi.Models;
using Search = MongoDB.Entities.Search;

namespace WebApi.Repositories;

public class SearchRepository : ISearchRepository
{
    public async Task<SearchResult> SearchItems(Query query, CancellationToken token)
    {
        var count = await DB.CountAsync<Item>(cancellation:token);
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

    public async Task DeleteItem(Guid id)
    {
        await DB.DeleteAsync<Item>(id);
    }

    public async Task UpdateItem(Guid id, string make, string model, string color, int mileage, int year)
    {
        await DB.Update<Item>()
            .MatchID(id)
            .Modify(p => p.Make, make)
            .Modify(p => p.Model, model)
            .Modify(p => p.Color, color)
            .Modify(p => p.Mileage, mileage)
            .Modify(p => p.Year, year)
            .ExecuteAsync();
    }

    public async Task MarkFinished(Guid id, string status, string winner, int? soldAmount)
    {
        await DB.Update<Item>()
            .MatchID(id)
            .Modify(p => p.Status, status)
            .Modify(p => p.Winner, winner)
            .Modify(p => p.SoldAmount, soldAmount)
            .ExecuteAsync();
    }
}