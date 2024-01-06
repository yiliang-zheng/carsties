using Ardalis.Specification;

namespace Domain.Auction.Specification;

public class AuctionSearchSpec : Specification<Auction>
{
    public AuctionSearchSpec(DateTime? fromDate)
    {
        if (fromDate is not null && fromDate != default(DateTime))
        {
            var utcDate = DateTime.SpecifyKind(fromDate.Value, DateTimeKind.Utc);
            Query.Where(p => (p.UpdatedAt != null &&
                              p.UpdatedAt >= utcDate) ||
                             (p.UpdatedAt == null &&
                              p.CreatedAt >= utcDate)
            );
        }

        Query.Include(p => p.Item).AsSplitQuery();
    }
}