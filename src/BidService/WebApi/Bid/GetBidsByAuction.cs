using Application.ListBids;
using MediatR;

namespace WebApi.Bid;

public class GetBidsByAuction() : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/{auctionId}", async (ISender sender, Guid auctionId) =>
        {
            var command = new ListBidsCommand(auctionId);
            var result = await sender.Send(command);
            if (result.IsFailed) return Results.BadRequest(result.Errors);

            return Results.Ok(result.Value);
        })
        .AllowAnonymous();
    }
}