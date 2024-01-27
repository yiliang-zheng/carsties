using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Domain.Auction;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Interface;

namespace Infrastructure.Data.Repository;

public class AuctionRepository : IRepository<Auction>
{
    private readonly AppDbContext _dbContext;

    public AuctionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        
    }
    public async Task<Auction> GetAsync(ISpecification<Auction> specification, CancellationToken cancellationToken = default)
    {
        var result = await this.ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
        return result;
    }

    public async Task<List<Auction>> ListAsync(ISpecification<Auction> specification, CancellationToken cancellationToken = default)
    {
        var result = await this.ApplySpecification(specification).ToListAsync(cancellationToken);
        return result;
    }

    public async Task<Auction> AddAsync(Auction entity, CancellationToken cancellationToken = default)
    {
        await this._dbContext.Auctions.AddAsync(entity, cancellationToken);
        return entity;
    }

    public Task UpdateAsync(Auction entity, CancellationToken cancellationToken = default)
    {
        this._dbContext.Auctions.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Auction entity, CancellationToken cancellationToken = default)
    {
        var items = await this._dbContext.Items.Where(p => p.AuctionId == entity.Id).ToListAsync(cancellationToken);
        if (items.Count > 0)
        {
            this._dbContext.Items.RemoveRange(items);
        }
        this._dbContext.Auctions.Remove(entity);
    }

    private IQueryable<Auction> ApplySpecification(ISpecification<Auction> specification)
    {
        return SpecificationEvaluator.Default.GetQuery(this._dbContext.Set<Auction>().AsQueryable(), specification);
    }
}