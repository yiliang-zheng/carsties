using Domain.Auction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class AuctionConfig:IEntityTypeConfiguration<Auction>
{
    public void Configure(EntityTypeBuilder<Auction> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p=>p.Seller).HasMaxLength(200).IsRequired();
        builder.Property(p => p.AuctionStatus)
            .HasConversion(
            p => p.Value, 
            p => Status.FromValue(p, Status.Live));
        builder.HasOne(p => p.Item)
            .WithOne(p => p.Auction)
            .HasForeignKey<Item>(p => p.AuctionId)
            .IsRequired();
    }
}