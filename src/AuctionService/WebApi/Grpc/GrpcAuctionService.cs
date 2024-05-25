using System.Globalization;
using Application.GetAuction;
using AuctionServices;
using Grpc.Core;
using MediatR;

namespace WebApi.Grpc;

public class GrpcAuctionService(ISender sender,ILogger<GrpcAuctionService> logger):GrpCAuctions.GrpCAuctionsBase
{
    public override async Task<GetAuctionResponse> GetAuction(GetAuctionRequest request, ServerCallContext context)
    {
        logger.LogInformation("--> Receive Grpc request: {Request}", request.Id);
        var command = new GetAuctionCommand(Guid.Parse(request.Id));
        var auction = await sender.Send(command);
        if (auction.IsFailed) throw new RpcException(new Status(StatusCode.NotFound, "Auction not found."));

        var result = new GetAuctionResponse
        {
            Auction = new AuctionModel
            {
                Id = auction.Value.Id.ToString(),
                Seller = auction.Value.Seller,
                ReservePrice = auction.Value.ReservePrice,
                AuctionEnd = auction.Value.AuctionEnd.ToString("O", CultureInfo.InvariantCulture),
                Finished = auction.Value.Status.Equals(Domain.Auction.Status.Finished.Name) || auction.Value.Status.Equals(Domain.Auction.Status.ReserveNotMet.Name),
                ItemSold = auction.Value.SoldAmount > 0,
                Winner = auction.Value.Winner
            }
        };
        return result;
    }
}