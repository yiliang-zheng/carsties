using Shared.Domain.Interface;

namespace Infrastructure.Data.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await this._dbContext.SaveChangesAsync(cancellationToken);
    }
}