using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Domain.Bid;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Interface;

namespace Infrastructure.Repository;

public class BidRepository:IRepository<Bid>
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

    private IQueryable<Bid> ApplySpecification(ISpecification<Bid> specification)
    {
        return SpecificationEvaluator.Default.GetQuery(this._context.Set<Bid>().AsQueryable(), specification);
    }
}