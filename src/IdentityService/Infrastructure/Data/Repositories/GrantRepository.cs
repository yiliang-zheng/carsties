using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Domain.Grant;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Interface;

namespace Infrastructure.Data.Repositories;

public class GrantRepository(ApplicationDbContext dbContext):IRepository<Grant>
{
    public async Task<Grant> GetAsync(ISpecification<Grant> specification, CancellationToken cancellationToken = default)
    {
        var result = await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
        return result;
    }

    public async Task<List<Grant>> ListAsync(ISpecification<Grant> specification, CancellationToken cancellationToken = default)
    {
        var result = await ApplySpecification(specification).ToListAsync(cancellationToken);
        return result;
    }

    public async Task<Grant> AddAsync(Grant entity, CancellationToken cancellationToken = default)
    {
        await dbContext.Grants.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(Grant entity, CancellationToken cancellationToken = default)
    {
        dbContext.Grants.Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Grant entity, CancellationToken cancellationToken = default)
    {
        dbContext.Grants.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<Grant> entities, CancellationToken cancellationToken = default)
    {
        dbContext.Grants.RemoveRange(entities);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<Grant> ApplySpecification(ISpecification<Grant> specification)
    {
        return SpecificationEvaluator.Default.GetQuery(dbContext.Set<Grant>().AsQueryable(), specification);
    }
}