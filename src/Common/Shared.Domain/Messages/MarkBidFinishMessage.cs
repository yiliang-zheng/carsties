using MassTransit;

namespace Shared.Domain.Messages;

public record MarkBidFinishMessage(Guid CorrelationId, Guid AuctionId) : CorrelatedBy<Guid>
{

}