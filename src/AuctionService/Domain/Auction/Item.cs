using Ardalis.GuardClauses;
using Shared.Domain;

namespace Domain.Auction;

public class Item : EntityBase
{
    public string Make { get; private set; }

    public string Model { get; private set; }

    public int Year { get; private set; }

    public string Color { get; private set; }

    public int Mileage { get; private set; }

    public string ImageUrl { get; private set; }

    public Guid AuctionId { get; private set; }

    public Auction Auction { get; private set; }

    public Item(string make, string model, int year, string color, int mileage, string imageUrl)
    {
        Make = Guard.Against.NullOrEmpty(make, nameof(make));
        Model = Guard.Against.NullOrEmpty(model, nameof(model));
        Year = Guard.Against.NegativeOrZero(year, nameof(year));
        Color = Guard.Against.NullOrEmpty(color, nameof(color));
        Mileage = Guard.Against.NegativeOrZero(mileage, nameof(mileage));
        ImageUrl = Guard.Against.NullOrEmpty(imageUrl, nameof(imageUrl));
    }

    public void UpdateItem(string make, string model, string color, int? mileage, int? year)
    {
        if (!string.IsNullOrEmpty(make)) this.Make = make;
        if(!string.IsNullOrEmpty(model)) this.Model = model;
        if(!string.IsNullOrEmpty(color)) this.Color = color;
        if(mileage > 0) this.Mileage = mileage.Value;
        if(year > 0) this.Year = year.Value;
    }
}