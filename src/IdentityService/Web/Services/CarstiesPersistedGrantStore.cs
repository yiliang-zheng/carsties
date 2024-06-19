using AutoMapper;
using Domain.Grant.Specifications;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using Shared.Domain.Interface;

namespace Web.Services;

public class CarstiesPersistedGrantStore(IRepository<Domain.Grant.Grant> grantRepo, IMapper mapper) : IPersistedGrantStore
{
    public async Task StoreAsync(PersistedGrant grant)
    {
        var entity = mapper.Map<Domain.Grant.Grant>(grant);
        await grantRepo.AddAsync(entity);
    }

    public async Task<PersistedGrant> GetAsync(string key)
    {
        var spec = new GrantByKeySpec(key);
        var entity = await grantRepo.GetAsync(spec);
        var result = mapper.Map<PersistedGrant>(entity);
        return result;
    }

    public async Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
    {
        var spec = new GrantsByFilterSpec(filter);
        var entities = await grantRepo.ListAsync(spec);
        var result = entities.Select(mapper.Map<PersistedGrant>).ToList();
        return result;
    }

    public async Task RemoveAsync(string key)
    {
        var spec = new GrantByKeySpec(key);
        var entity = await grantRepo.GetAsync(spec);
        if (entity is not null)
        {
            await grantRepo.DeleteAsync(entity);
        }
    }

    public Task RemoveAllAsync(PersistedGrantFilter filter)
    {
        throw new NotImplementedException();
    }
}