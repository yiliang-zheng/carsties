﻿namespace WebApi.Auction;

public class DeleteAuctionRequest
{
    public const string Route = "/api/auction/{Id}";

    public Guid Id { get; set; }
};