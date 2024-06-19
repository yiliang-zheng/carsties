using AutoMapper;
using Duende.IdentityServer.Models;

namespace Web.Profiles;

public class PersistedGrantProfile : Profile
{
    public PersistedGrantProfile()
    {
        CreateMap<Domain.Grant.Grant, PersistedGrant>();

        CreateMap<PersistedGrant, Domain.Grant.Grant>()
            .ForMember(dest => dest.SessionId,
                opts => opts.NullSubstitute(string.Empty))
            .ForMember(dest => dest.Description,
                opts => opts.MapFrom(src=>src.Description ?? string.Empty));
    }
}