using Ardalis.Specification;
using Duende.IdentityServer.Stores;

namespace Domain.Grant.Specifications;

public sealed class GrantsByFilterSpec:Specification<Grant>
{
    public GrantsByFilterSpec(PersistedGrantFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.SessionId)) Query.Where(p => p.SessionId == filter.SessionId);

        if (!string.IsNullOrEmpty(filter.ClientId)) Query.Where(p => p.ClientId == filter.ClientId);

        if (!string.IsNullOrEmpty(filter.SubjectId)) Query.Where(p => p.SubjectId == filter.SubjectId);

        if (!string.IsNullOrEmpty(filter.Type)) Query.Where(p => p.Type == filter.Type);

        if (filter.ClientIds is not null && filter.ClientIds.Any())
            Query.Where(p => filter.ClientIds.Contains(p.ClientId));

        if (filter.Types is not null && filter.Types.Any()) Query.Where(p => filter.Types.Contains(p.Type));
    }
}