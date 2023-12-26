using Ardalis.Specification;

namespace Domain.Auction.Specification;

public class AuctionByIdSpec : SingleResultSpecification<Auction>
{
    public AuctionByIdSpec(Guid id)
    {
        Query.Where(p => p.Id == id)
            .Include(p => p.Item)
            .AsSplitQuery();
    }
}