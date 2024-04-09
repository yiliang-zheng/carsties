using Ardalis.SmartEnum;

namespace Domain.Bid;

public sealed class BidStatus: SmartEnum<BidStatus>
{
    public static readonly BidStatus Accepted = new BidStatus(nameof(Accepted), 1);

    public static readonly BidStatus AcceptedBelowReserve = new BidStatus("Accepted Below Reserve", 2);

    public static readonly BidStatus TooLow = new BidStatus("Too Low", 3);

    public static readonly BidStatus Finished = new BidStatus(nameof(Finished), 4);
    public BidStatus(string name, int value) : base(name, value)
    {
    }
}