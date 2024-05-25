using MassTransit;

namespace Shared.Domain.Events;

public record MarkAuctionFinishFailed(Guid AuctionId, Guid CorrelationId) : CorrelatedBy<Guid>;