using Ardalis.Specification;

namespace Domain.Auction.Specification;

public class FinishAuctionSearchSpec:Specification<Auction>
{
    public FinishAuctionSearchSpec()
    {
        Query.Where(p => p.AuctionEnd <= DateTime.UtcNow && p.AuctionStatus == Status.Live);
    }
}