using Domain.Bid;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class BidConfig:IEntityTypeConfiguration<Bid>
{
    public void Configure(EntityTypeBuilder<Bid> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.BidStatus).HasConversion(
            domain => domain.Value,
            entity => BidStatus.FromValue(entity));

        builder.HasOne(p => p.Auction)
            .WithMany()
            .HasForeignKey(p => p.AuctionId);
    }
}