using MongoDB.Bson;
using MongoDB.Driver;
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
        PipelineStageDefinition<Item, ItemDateTime> addFields = new BsonDocument
        {
            {
                "$addFields", new BsonDocument
                {
                    {
                        nameof(ItemDateTime.CreatedOrUpdatedAt),  new BsonDocument
                        {
                            {
                                "$cond", new BsonDocument
                                {
                                    {
                                        "if", new BsonDocument
                                        {
                                            {
                                                "$ne", new BsonArray
                                                {
                                                    $"${nameof(Item.UpdatedAt)}",
                                                    default(DateTime?)
                                                }
                                            }
                                        }
                                    },
                                    {
                                        "then", $"${nameof(Item.UpdatedAt)}"
                                    },
                                    {
                                        "else", $"${nameof(Item.CreatedAt)}"
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        var result = await DB.Fluent<Item>()
            .AppendStage(addFields)
            .SortByDescending(p => p.CreatedOrUpdatedAt)
            .FirstOrDefaultAsync();

        //var lastUpdated = await DB.Find<Item, DateTime>()
        //    .Sort(p => p.UpdatedAt.HasValue ? p.UpdatedAt.Value : p.CreatedAt, Order.Descending)
        //    .Project(p => p.UpdatedAt.HasValue ? p.UpdatedAt.Value : p.CreatedAt)
        //    .ExecuteFirstAsync();

        var auctionUrl = _configuration["AuctionServiceUrl"];
        var auctions = await _httpClient.GetFromJsonAsync<List<Item>>($"{auctionUrl}/api/auction?from={result?.CreatedOrUpdatedAt}");

        return auctions;
    }
}