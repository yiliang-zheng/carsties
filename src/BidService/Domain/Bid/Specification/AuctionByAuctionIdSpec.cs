using Ardalis.Specification;

namespace Domain.Bid.Specification;

public class AuctionByAuctionIdSpec:SingleResultSpecification<Auction>
{
    public AuctionByAuctionIdSpec( Guid auctionId)
    {
        Query.Where(p => p.Id == auctionId);
    }
}