using Domain.Auction;
using Domain.Auction.Event;
using Domain.Auction.Specification;
using FluentResults;
using MediatR;
using Shared.Domain.Interface;

namespace Application.DeleteAuction;

public class DeleteAuctionCommandHandler : IRequestHandler<DeleteAuctionCommand, Result>
{
    private readonly IRepository<Auction> _repository;

    public DeleteAuctionCommandHandler(IRepository<Auction> repository)
    {
        _repository = repository;
    }
    public async Task<Result> Handle(DeleteAuctionCommand request, CancellationToken cancellationToken)
    {
        var spec = new AuctionByIdSpec(request.Id);
        var auction = await this._repository.GetAsync(spec, cancellationToken);
        if (auction is null) return Result.Fail("auction not found");

        auction.MarkDeleted();
        await this._repository.DeleteAsync(auction, cancellationToken);
        return Result.Ok();
    }
}