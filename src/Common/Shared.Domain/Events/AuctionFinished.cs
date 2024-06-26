﻿namespace Shared.Domain.Events;

public record AuctionFinished
{
    public Guid AuctionId { get; init; }
    public bool ItemSold { get; init; }

    public string Winner { get; init; }

    public string Seller { get; init; }

    public int? Amount { get; init; }
};