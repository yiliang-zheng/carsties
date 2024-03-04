namespace Shared.Domain.Events;

public class SearchMarkFinished
{
    public Guid CorrelationId { get; init; }

    public Guid AuctionId { get; init; }

    public DateTimeOffset CreatedDate { get; init; }
}


public class SearchMarkFinishedFailed
{
    public Guid CorrelationId { get; init; }

    public Guid AuctionId { get; init; }

    public DateTimeOffset CreatedDate { get; init; }

    public Exception FailedException { get; init; }
}