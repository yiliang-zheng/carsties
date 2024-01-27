﻿using FastEndpoints;

namespace WebApi.Auction;

public class UpdateAuctionRequest
{
    public const string Route = "/api/auction/{Id}";

    public Guid Id { get; set; }

    public string Make { get; set; }

    public string Model { get; set; }

    public string Color { get; set; }

    public int? Mileage { get; set; }

    public int? Year { get; set; }
}