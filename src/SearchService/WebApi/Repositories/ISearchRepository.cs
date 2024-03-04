using WebApi.Endpoints;
using WebApi.Models;

namespace WebApi.Repositories;

public interface ISearchRepository
{
    Task<SearchResult> SearchItems(Query query, CancellationToken token);

    Task DeleteItem(Guid id);

    Task UpdateItem(Guid id, string make, string model, string color, int mileage, int year);

    Task MarkFinished(Guid id, string status, string winner, int? soldAmount);
}