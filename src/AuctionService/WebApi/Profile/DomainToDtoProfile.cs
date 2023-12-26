using Application;
using Domain.Auction;

namespace WebApi.Profile;

public class DomainToDtoProfile : AutoMapper.Profile
{
    public DomainToDtoProfile()
    {
        CreateMap<Domain.Auction.Auction, AuctionDto>()
            .ForCtorParam(
                "Make",
                opts => opts.MapFrom(src => src.Item == null ? string.Empty : src.Item.Make)
            )
            .ForMember(
                dest => dest.Model,
                opts => opts.MapFrom(src => src.Item == null ? string.Empty : src.Item.Model)
            )
            .ForMember(
                dest => dest.Year,
                opts => opts.MapFrom(src => src.Item == null ? 0 : src.Item.Year)
            )
            .ForMember(
                dest => dest.Color,
                opts => opts.MapFrom(src => src.Item == null ? string.Empty : src.Item.Color)
            )
            .ForMember(
                dest => dest.Mileage,
                opts => opts.MapFrom(src => src.Item == null ? default(int) : src.Item.Mileage)
            )
            .ForMember(
                dest => dest.ImageUrl,
                opts => opts.MapFrom(src => src.Item == null ? string.Empty : src.Item.ImageUrl)
            )
            .ForMember(
                dest => dest.Status,
                opts => opts.MapFrom(src => src.AuctionStatus.Name)
            );

    }
}