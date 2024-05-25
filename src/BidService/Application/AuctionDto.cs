namespace Application;

public sealed class AuctionDto
{
    public Guid AuctionId { get; init; }

    public string Seller { get; init; } = null!;

    public bool ItemSold { get; init; }

    public string Winner { get; init; } = null!;

    public int? SoldAmount { get; set; }

    public bool Finished { get; init; }

    public int ReservePrice { get; init; }

    public DateTimeOffset AuctionEnd { get; init; }
}