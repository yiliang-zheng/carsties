using Ardalis.Specification;

namespace Shared.Domain.Interface;

public interface IRepository<T> where T: EntityBase
{
    Task<T> GetAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}