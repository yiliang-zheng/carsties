using System.Linq.Expressions;
using System.Reflection;
using Ardalis.GuardClauses;
using Domain.Auction.Event;
using Shared.Domain;
using Shared.Domain.Interface;

namespace Domain.Auction;

public class Auction : EntityBase, IAggregateRoot, IAuditableEntity
{
    public int ReservePrice { get; private set; }

    public string Seller { get; private set; }

    public string Winner { get; private set; }

    public int? SoldAmount { get; private set; }

    public int? CurrentHighBid { get; private set; }

    public DateTime AuctionEnd { get; private set; }

    public Status AuctionStatus { get; private set; } = Status.Live;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Item Item { get; private set; }

    public Auction(int reservePrice, string seller, string winner, int? soldAmount, int? currentHighBid, DateTime auctionEnd)
    {
        ReservePrice = Guard.Against.NegativeOrZero(reservePrice, nameof(reservePrice));
        Seller = Guard.Against.NullOrEmpty(seller, nameof(seller));
        Winner = winner;
        SoldAmount = soldAmount;
        CurrentHighBid = currentHighBid;
        AuctionEnd = Guard.Against.Default(auctionEnd, nameof(auctionEnd));
    }

    public void AddItem(string make, string model, int year, string color, int mileage, string imageUrl)
    {
        var item = new Item(
            make,
            model,
            year,
            color,
            mileage,
            imageUrl
        );
        this.Item = item;
    }

    public void UpdateAuction(string make, string model, string color, int? mileage, int? year)
    {
        if (Item == null) return;
        Item.UpdateItem(
            make,
            model,
            color,
            mileage,
            year
        );
        this.RegisterDomainEvent(new AuctionUpdated(
            this.Id,
            this.Item.Make,
            this.Item.Model,
            this.Item.Color,
            this.Item.Mileage,
            this.Item.Year
            ));
    }

    public void MarkDeleted()
    {
        this.RegisterDomainEvent(new AuctionDeleted(this.Id));
    }

    public void UpdateStatus(Status newStatus)
    {
        this.AuctionStatus = newStatus;
    }
}