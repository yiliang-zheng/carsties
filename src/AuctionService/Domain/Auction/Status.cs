using Ardalis.SmartEnum;

namespace Domain.Auction;

public class Status : SmartEnum<Status>
{
    public static readonly Status Live = new(nameof(Live), 1);
    public static readonly Status Finished = new(nameof(Finished), 2);
    public static readonly Status ReserveNotMet = new("Reserve Not Met", 3);
    public Status(string name, int value) : base(name, value)
    {
    }
}