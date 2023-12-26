using Ardalis.Specification;

namespace Domain.Auction.Specification;

public class AuctionSearchSpec : Specification<Auction>
{
    public AuctionSearchSpec(DateTime? fromDate)
    {
        if (fromDate is not null && fromDate != default(DateTime))
        {
            Query.Where(p => (p.UpdatedAt != null &&
                              p.UpdatedAt >= fromDate) ||
                             (p.UpdatedAt == null &&
                              p.CreatedAt >= fromDate)
            );
        }

        Query.Include(p => p.Item).AsSplitQuery();
    }
}