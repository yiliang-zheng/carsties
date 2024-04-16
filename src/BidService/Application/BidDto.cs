namespace Application
{
    public sealed class BidDto
    {
        public Guid Id { get; init; }

        public DateTimeOffset BidDateTime { get; init; }

        public int Amount { get; init; }

        public string BidStatus { get; init; } = null!;

        public Guid AuctionId { get; init; }

        public DateTimeOffset AuctionEnd { get; init; }

        public string Seller { get; init; } = null!;

        public int ReservePrice { get; init; }

        public bool Finished { get; init; }

    }
}
