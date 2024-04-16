using Ardalis.Specification;

namespace Domain.Bid.Specification;

public class BidsByAuctionSpec: Specification<Bid>
{
    public BidsByAuctionSpec(Guid auctionId)
    {
        Query
            .Where(p=>p.AuctionId == auctionId)
            .Include(p => p.Auction)
            .AsSplitQuery();
    }
}