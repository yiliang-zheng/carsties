using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using WebApi.Models;
using WebApi.Service;

namespace WebApi.Data;

public class DatabaseInitializer
{
    public static async Task InitDb(WebApplication app)
    {

        await DB.InitAsync("SearchDb",
            MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Item>()
            .Key(p => p.Make, KeyType.Text)
            .Key(p => p.Model, KeyType.Text)
            .Key(p => p.Color, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<Item>();
        using var scope = app.Services.CreateScope();
        var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();
        if (httpClient is not null)
        {
            var auctions = await httpClient.GetItemsForSearchDb();
            if (auctions is { Count: > 0 }) await DB.SaveAsync(auctions);
        }
    }
}