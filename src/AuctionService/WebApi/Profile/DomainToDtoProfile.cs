using Application;
using Domain.Auction;

namespace WebApi.Profile;

public class DomainToDtoProfile : AutoMapper.Profile
{
    public DomainToDtoProfile()
    {
        CreateMap<Domain.Auction.Auction, AuctionDto>()
            .ForCtorParam(
                "Id",
                opts => opts.MapFrom(src => src.Id)
            )
            .ForCtorParam(
                "CreatedAt",
                opts => opts.MapFrom(src => src.CreatedAt)
            )
            .ForCtorParam(
                "UpdatedAt",
                opts => opts.MapFrom(src => src.UpdatedAt)
            )
            .ForCtorParam(
                "AuctionEnd",
                opts => opts.MapFrom(src => src.AuctionEnd)
            )
            .ForCtorParam(
                "Seller",
                opts => opts.MapFrom(src => src.Seller)
            )
            .ForCtorParam(
                "Winner",
                opts => opts.MapFrom(src => src.Winner)
            )
            .ForCtorParam(
                "Make",
                opts => opts.MapFrom(src => src.Item == null ? string.Empty : src.Item.Make)
            )
            .ForCtorParam(
                "Model",
                opts => opts.MapFrom(src => src.Item == null ? string.Empty : src.Item.Model)
            )
            .ForCtorParam(
                "Year",
                opts => opts.MapFrom(src => src.Item == null ? 0 : src.Item.Year)
            )
            .ForCtorParam(
                "Color",
                opts => opts.MapFrom(src => src.Item == null ? string.Empty : src.Item.Color)
            )
            .ForCtorParam(
                "Mileage",
                opts => opts.MapFrom(src => src.Item == null ? default(int) : src.Item.Mileage)
            )
            .ForCtorParam(
                "ImageUrl",
                opts => opts.MapFrom(src => src.Item == null ? string.Empty : src.Item.ImageUrl)
            )
            .ForCtorParam(
                "Status",
                opts => opts.MapFrom(src => src.AuctionStatus.Name)
            )
            .ForCtorParam(
                "ReservePrice",
                opts => opts.MapFrom(src => src.ReservePrice)
            )
            .ForCtorParam(
                "SoldAmount",
                opts => opts.MapFrom(src => src.SoldAmount)
            )
            .ForCtorParam(
                "CurrentHighBid",
                opts => opts.MapFrom(src => src.CurrentHighBid)
            );
    }
}