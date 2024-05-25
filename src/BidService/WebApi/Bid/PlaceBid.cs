using Application.PlaceBid;
using Domain.Bid.Errors;
using MediatR;

namespace WebApi.Bid;

public class PlaceBid: IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/create", async (ISender sender, PlaceBideRequest request) =>
            {
                var command = new PlaceBidCommand(request.Bidder, request.Amount, request.AuctionId);
                var result = await sender.Send(command);
                if (result.IsSuccess) return Results.Ok(result.Value);

                var error = result.Errors?.FirstOrDefault()!;

                
                return error == BidErrors.AuctionNotFound ? Results.NotFound() : Results.BadRequest(error.Message);
            })
            .RequireAuthorization();
    }
}