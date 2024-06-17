using Domain.Bid;

namespace Application.Profile;

public class DomainToDtoProfile:AutoMapper.Profile
{
    public DomainToDtoProfile()
    {
        CreateMap<Bid, BidDto>()
            .ForMember(dest => dest.BidStatus,
                opts => opts.MapFrom(src => src.BidStatus.Name)
            )
            .ForMember(dest => dest.AuctionEnd,
                opts => opts.MapFrom(src => src.Auction.AuctionEnd)
            )
            .ForMember(dest => dest.Seller,
                opts => opts.MapFrom(src => src.Auction.Seller)
            )
            .ForMember(dest => dest.ReservePrice,
                opts => opts.MapFrom(src => src.Auction.ReservePrice)
            )
            .ForMember(dest => dest.Finished,
                opts => opts.MapFrom(src => src.Auction.Finished)
            )
            .ForMember(dest => dest.BidDateTime,
                opts => opts.MapFrom(src => src.BidTime));

        CreateMap<Auction, AuctionDto>()
            .ForMember(dest => dest.AuctionId,
                opts => opts.MapFrom(src => src.Id));
    }
}