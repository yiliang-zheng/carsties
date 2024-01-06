using AutoMapper;
using Shared.Domain.Events;
using WebApi.Models;

namespace WebApi.Profiles;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<AuctionCreated, Item>();
    }
}