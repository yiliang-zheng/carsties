using FastEndpoints;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Endpoints;

public class Search:Endpoint<SearchItemRequest, SearchResult>
{
    private readonly ISearchRepository _search;

    public Search(ISearchRepository search)
    {
        _search = search;
    }

    public override void Configure()
    {
        Get(SearchItemRequest.Route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(SearchItemRequest req, CancellationToken ct)
    {
        if (req?.Query is null)
        {
            AddError(p=>p.Query, "query is empty");
            await SendErrorsAsync(400, ct);
            return;
        }

        var result = await this._search.SearchItems(req.Query, ct);
        await SendOkAsync(result, ct);
    }
}