using Domain.Auction;
using Domain.Auction.Specification;
using FluentResults;
using MediatR;
using Shared.Domain.Interface;

namespace Application.DeleteAuction;

public class DeleteAuctionCommandHandler(IRepository<Auction> repository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteAuctionCommand, Result>
{
    public async Task<Result> Handle(DeleteAuctionCommand request, CancellationToken cancellationToken)
    {
        var spec = new AuctionByIdSpec(request.Id);
        var auction = await repository.GetAsync(spec, cancellationToken);
        if (auction is null) return Result.Fail("auction not found");

        if (!auction.Seller.Equals(request.Seller))
            return Result.Fail("invalid request. Auction seller is not the same.");

        auction.MarkDeleted();
        await repository.DeleteAsync(auction, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }
}