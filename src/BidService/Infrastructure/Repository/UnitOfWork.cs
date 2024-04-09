using Shared.Domain.Interface;

namespace Infrastructure.Repository;

public class UnitOfWork(AppDbContext dbContext):IUnitOfWork
{
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}