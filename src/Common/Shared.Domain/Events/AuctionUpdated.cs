namespace Shared.Domain.Events;

public class AuctionUpdated : DomainEventBase
{
    public Guid Id { get; set; }

    public string Make { get; set; }

    public string Model { get; set; }

    public string Color { get; set; }

    public int Mileage { get; set; }

    public int Year { get; set; }

    public void Deconstruct(out Guid id,out string make, out string model, out string color, out int mileage, out int year)
    {
        id = Id;
        make = Make;
        model = Model;
        color = Color;
        mileage = Mileage;
        year = Year;
    }

}