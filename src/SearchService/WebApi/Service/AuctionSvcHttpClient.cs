using MongoDB.Entities;
using WebApi.Models;

namespace WebApi.Service;

public class AuctionSvcHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    public AuctionSvcHttpClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }
    public async Task<List<Item>> GetItemsForSearchDb()
    {
        var lastUpdated = await DB.Find<Item, DateTime?>()
            .Sort(p => p.Descending(x => x.UpdatedAt))
            .Project(p => p.UpdatedAt)
            .ExecuteFirstAsync();

        var auctionUrl = _configuration["AuctionServiceUrl"];
        var auctions = await _httpClient.GetFromJsonAsync<List<Item>>($"{auctionUrl}/api/auction?from={lastUpdated}");

        return auctions;
    }
}