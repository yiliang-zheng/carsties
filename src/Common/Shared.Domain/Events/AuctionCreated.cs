namespace Shared.Domain.Events;

//public record AuctionCreated(
//    Guid Id,
//    DateTime CreatedAt,
//    DateTime? UpdatedAt,
//    DateTime AuctionEnd,
//    string Seller,
//    string Winner,
//    string Make,
//    string Model,
//    int Year,
//    string Color,
//    int Mileage,
//    string ImageUrl,
//    string Status,
//    int ReservePrice,
//    int? SoldAmount,
//    int? CurrentHighBid
//    ) : DomainEventBase;

public class AuctionCreated : DomainEventBase
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime AuctionEnd { get; set; }

    public string Seller { get; set; }

    public string Winner { get; set; }

    public string  Make { get; set; }

    public string Model { get; set; }

    public int Year { get; set; }

    public string Color { get; set; }

    public int Mileage { get; set; }

    public string ImageUrl { get; set; }
    public string Status { get; set; }

    public int ReservePrice { get; set; }

    public int? SoldAmount { get; set; }

    public int? CurrentHighBid { get; set; }
}