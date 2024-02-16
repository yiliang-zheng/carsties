using Ardalis.SmartEnum;

namespace Domain.Auction;

public sealed class BidStatus : SmartEnum<BidStatus>
{
    public static readonly BidStatus Accepted = new(nameof(Accepted), 1);
    public BidStatus(string name, int value) : base(name, value)
    {
    }
}