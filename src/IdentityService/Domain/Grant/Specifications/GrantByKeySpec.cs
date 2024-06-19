using Ardalis.Specification;

namespace Domain.Grant.Specifications;

public sealed class GrantByKeySpec:Specification<Grant>
{
    public GrantByKeySpec(string key)
    {
        Query.Where(p => p.Key == key);
    }
}