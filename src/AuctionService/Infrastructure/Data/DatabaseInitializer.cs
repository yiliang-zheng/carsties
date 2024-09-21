using Domain.Auction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public class DatabaseInitializer(AppDbContext dbContext, ILogger<DatabaseInitializer> logger)
{
    public async Task Initialize()
    {
        try
        {
            await dbContext.Database.MigrateAsync();

            // 1 Ford GT
            var fordGt = new Auction(
                id: Guid.NewGuid(), 
                reservePrice: 20000,
                seller: "bob",
                winner: null,
                soldAmount: null,
                currentHighBid: null,
                auctionEnd: DateTime.UtcNow.AddDays(10));
            fordGt.AddItem(
                "Ford",
                "GT",
                2020,
                "White",
                50000,
                "https://cdn.pixabay.com/photo/2016/05/06/16/32/car-1376190_960_720.jpg"
                );
            await this.SeedData(fordGt);

            // 2 Bugatti Veyron
            var bugatti = new Auction(
                id: Guid.NewGuid(),
                reservePrice: 90000,
                seller: "alice",
                winner: null,
                soldAmount: null,
                currentHighBid: null,
                auctionEnd: DateTime.UtcNow.AddDays(60)
            );
            bugatti.AddItem(
                "Bugatti",
                "Veyron",
                2018,
                "Black",
                15035,
                "https://cdn.pixabay.com/photo/2012/05/29/00/43/car-49278_960_720.jpg"
                );
            await this.SeedData(bugatti);

            // 3 Ford mustang
            var fordMustang = new Auction(
                id: Guid.NewGuid(),
                reservePrice: 40000,
                seller: "bob",
                winner: null,
                soldAmount: null,
                currentHighBid: null,
                auctionEnd: DateTime.UtcNow.AddDays(4)
            );
            fordMustang.AddItem(
                "Ford",
                "Mustang",
                2023,
                "Black",
                65125,
                "https://cdn.pixabay.com/photo/2012/11/02/13/02/car-63930_960_720.jpg"
            );
            await this.SeedData(fordMustang);

            // 4 Mercedes SLK
            var mercedes = new Auction(
                id: Guid.NewGuid(),
                reservePrice: 50000,
                seller: "tom",
                winner: null,
                soldAmount: null,
                currentHighBid: null,
                auctionEnd: DateTime.UtcNow.AddDays(-10)
            );
            mercedes.UpdateStatus(Status.ReserveNotMet);
            mercedes.AddItem(
                "Mercedes",
                "SLK",
                2020,
                "Silver",
                15001,
                "https://cdn.pixabay.com/photo/2016/04/17/22/10/mercedes-benz-1335674_960_720.png"
            );
            await this.SeedData(mercedes);

            // 5 BMW X1
            var bmw = new Auction(
                id: Guid.NewGuid(),
                reservePrice: 20000,
                seller: "alice",
                winner: null,
                soldAmount: null,
                currentHighBid: null,
                auctionEnd: DateTime.UtcNow.AddDays(30)
            );
            bmw.AddItem(
                "BMW",
                "X1",
                2017,
                "White",
                90000,
                "https://cdn.pixabay.com/photo/2017/08/31/05/47/bmw-2699538_960_720.jpg"
            );
            await this.SeedData(bmw);

            // 6 Ferrari spider
            var ferrariSpider = new Auction(
                id: Guid.NewGuid(),
                reservePrice: 200000,
                seller: "bob",
                winner: null,
                soldAmount: null,
                currentHighBid: null,
                auctionEnd: DateTime.UtcNow.AddDays(45)
            );
            ferrariSpider.AddItem(
                "Ferrari",
                "Spider",
                2015,
                "Red",
                50000,
                "https://cdn.pixabay.com/photo/2017/11/09/01/49/ferrari-458-spider-2932191_960_720.jpg"
            );
            await this.SeedData(ferrariSpider);

            // 7 Ferrari F-430
            var ferrari430 = new Auction(
                id: Guid.NewGuid(),
                reservePrice: 150000,
                seller: "alice",
                winner: null,
                soldAmount: null,
                currentHighBid: null,
                auctionEnd: DateTime.UtcNow.AddDays(13)
            );
            ferrari430.AddItem(
                "Ferrari",
                "F-430",
                2022,
                "Red",
                5000,
                "https://cdn.pixabay.com/photo/2017/11/08/14/39/ferrari-f430-2930661_960_720.jpg"
            );
            await this.SeedData(ferrari430);

            // 8 Audi R8
            var audiR8 = new Auction(
                id: Guid.NewGuid(),
                reservePrice: 60000,
                seller: "bob",
                winner: null,
                soldAmount: null,
                currentHighBid: null,
                auctionEnd: DateTime.UtcNow.AddDays(19)
            );
            audiR8.AddItem(
                "Audi",
                "R8",
                2021,
                "White",
                10050,
                "https://cdn.pixabay.com/photo/2019/12/26/20/50/audi-r8-4721217_960_720.jpg"
            );
            await this.SeedData(audiR8);

            // 9 Audi TT
            var audiTt = new Auction(
                id: Guid.NewGuid(),
                reservePrice: 20000,
                seller: "tom",
                winner: null,
                soldAmount: null,
                currentHighBid: null,
                auctionEnd: DateTime.UtcNow.AddDays(20)
            );
            audiTt.AddItem(
                "Audi",
                "TT",
                2020,
                "Black",
                25400,
                "https://cdn.pixabay.com/photo/2016/09/01/15/06/audi-1636320_960_720.jpg"
            );
            await this.SeedData(audiTt);

            // 10 Ford Model T
            var fordModelT = new Auction(
                id: Guid.NewGuid(),
                reservePrice: 20000,
                seller: "bob",
                winner: null,
                soldAmount: null,
                currentHighBid: null,
                auctionEnd: DateTime.UtcNow.AddDays(48)
            );
            fordModelT.AddItem(
                "Ford",
                "Model T",
                1938,
                "Rust",
                150150,
                "https://cdn.pixabay.com/photo/2017/08/02/19/47/vintage-2573090_960_720.jpg"
            );
            await this.SeedData(fordModelT);

            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred during initialize database");
        }
    }

    private async Task SeedData(Auction auction)
    {
        if (await dbContext.Auctions.AnyAsync(p => p.Seller == auction.Seller &&
                                                         p.ReservePrice == auction.ReservePrice &&
                                                         p.Item.Make == auction.Item.Make &&
                                                         p.Item.Model == auction.Item.Model &&
                                                         p.Item.Color == auction.Item.Color &&
                                                         p.Item.Mileage == auction.Item.Mileage &&
                                                         p.Item.Year == auction.Item.Year))
            return;

        await dbContext.Auctions.AddAsync(auction);
    }
}