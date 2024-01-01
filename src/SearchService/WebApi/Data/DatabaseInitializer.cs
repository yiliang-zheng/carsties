using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using WebApi.Models;

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
        if (count == 0)
        {
            var itemData = await File.ReadAllTextAsync("Data/auctions.json");
            try
            {
                var items = JsonSerializer.Deserialize<List<Item>>(
                    itemData,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (items is { Count: > 0 }) await DB.SaveAsync(items);
            }
            catch (Exception)
            {
                //ignored
            }

        }
    }
}