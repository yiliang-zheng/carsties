using Ardalis.Specification;

namespace Domain.Bid.Specification;

public class BidByIdSpec : SingleResultSpecification<Bid>
{
    public BidByIdSpec(Guid id)
    {
        Query.Where(p => p.Id == id)
            .Include(p => p.Auction)
            .AsSplitQuery();
    }
}