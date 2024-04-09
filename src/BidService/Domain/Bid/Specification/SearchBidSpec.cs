using Ardalis.Specification;

namespace Domain.Bid.Specification;

public class SearchBidSpec: Specification<Bid>
{
    public SearchBidSpec()
    {
        Query.Include(p => p.Auction).AsSplitQuery();
    }
}