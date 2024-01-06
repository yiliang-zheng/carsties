﻿namespace WebApi.Auction;

public class CreateAuctionRequest
{
    public const string Route = "/api/auction";

    public string Make { get; set; }

    public string Model { get; set; }

    public string Color { get; set; }

    public int Mileage { get; set; }

    public int Year { get; set; }

    public int ReservePrice { get; set; }

    public string ImageUrl { get; set; }

    public DateTime AuctionEnd { get; set; }

    public string Seller { get; set; }
}