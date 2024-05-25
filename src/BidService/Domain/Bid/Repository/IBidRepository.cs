using Ardalis.Specification;
using Shared.Domain.Interface;

namespace Domain.Bid.Repository;

public interface IBidRepository: IRepository<Bid>
{
    Task<Auction?> GetAuctionById(ISpecification<Auction> spec);

    Task<Auction> CreateAuction(Auction auction);

    Task UpdateAuction(Auction auction);
}