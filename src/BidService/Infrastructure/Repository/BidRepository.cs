using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Domain.Bid;
using Domain.Bid.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class BidRepository : IBidRepository
{
    private readonly AppDbContext _context;

    public BidRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Bid> GetAsync(ISpecification<Bid> specification, CancellationToken cancellationToken = default)
    {
        var result = await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
        return result;
    }

    public async Task<List<Bid>> ListAsync(ISpecification<Bid> specification, CancellationToken cancellationToken = default)
    {
        var result = await ApplySpecification(specification).ToListAsync(cancellationToken);
        return result;
    }

    public async Task<Bid> AddAsync(Bid entity, CancellationToken cancellationToken = default)
    {
        await _context.Bids.AddAsync(entity, cancellationToken);
        return entity;
    }

    public Task UpdateAsync(Bid entity, CancellationToken cancellationToken = default)
    {
        _context.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Bid entity, CancellationToken cancellationToken = default)
    {
        _context.Bids.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<Auction?> GetAuctionById(ISpecification<Auction> spec)
    {
        var result = await ApplySpecification(spec).FirstOrDefaultAsync();
        return result;

    }

    public async Task<Auction> CreateAuction(Auction auction)
    {
        await _context.Auctions.AddAsync(auction);
        return auction;
    }

    public Task UpdateAuction(Auction auction)
    {
        _context.Update(auction);
        return Task.CompletedTask;
    }

    private IQueryable<T> ApplySpecification<T>(ISpecification<T> specification) where T: class
    {
        return SpecificationEvaluator.Default.GetQuery(this._context.Set<T>().AsQueryable(), specification);
    }

    
}