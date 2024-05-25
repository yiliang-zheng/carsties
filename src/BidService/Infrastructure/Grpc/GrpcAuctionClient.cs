using AuctionServices;
using Domain.Bid;
using Domain.Bid.Repository;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Grpc;

public class GrpcAuctionClient(ILogger<GrpcAuctionClient> logger, IConfiguration config): IGrpcAuctionClient
{
    public Auction? GetAuction(Guid id)
    {
        logger.LogInformation("--> Invoking Grpc Service");

        var channel = GrpcChannel.ForAddress(config["GrpcAuction"]!);
        var client = new GrpCAuctions.GrpCAuctionsClient(channel);

        var request = new GetAuctionRequest
        {
            Id = id.ToString()
        };

        try
        {
            var response = client.GetAuction(request);
            var auction = new Auction(
                id: Guid.Parse(response.Auction.Id),
                auctionEnd: DateTimeOffset.Parse(response.Auction.AuctionEnd),
                seller: response.Auction.Seller,
                reservePrice: response.Auction.ReservePrice,
                finished: response.Auction.Finished,
                itemSold: response.Auction.ItemSold,
                winner: response.Auction.Winner
            );
            return auction;
        }
        catch (Exception e)
        {
            logger.LogError("Error occurred while calling Grpc service: {Exception}", e);
            return null;
        }
    }
}