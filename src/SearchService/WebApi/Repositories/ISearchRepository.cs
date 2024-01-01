using WebApi.Endpoints;
using WebApi.Models;

namespace WebApi.Repositories;

public interface ISearchRepository
{
    Task<SearchResult> SearchItems(Query query, CancellationToken token);
}