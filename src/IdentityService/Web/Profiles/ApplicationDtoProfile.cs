using Application;
using Domain.ApplicationUser;

namespace Web.Profiles;

public class ApplicationDtoProfile : AutoMapper.Profile
{
    public ApplicationDtoProfile()
    {
        CreateMap<ApplicationUser, ApplicationUserDto>();
    }
}