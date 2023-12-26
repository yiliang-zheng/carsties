using Application.CreateAuction;
using Application.DeleteAuction;
using Application.GetAuction;
using Application.SearchAuction;
using Application.UpdateAuction;
using WebApi.Auction;

namespace WebApi.Profile;

public class RequestToCommandProfile : AutoMapper.Profile
{
    public RequestToCommandProfile()
    {
        CreateMap<CreateAuctionRequest, CreateAuctionCommand>();

        CreateMap<UpdateAuctionRequest, UpdateAuctionCommand>();

        CreateMap<DeleteAuctionRequest, DeleteAuctionCommand>();

        CreateMap<ListAuctionRequest, SearchAuctionCommand>();

        CreateMap<GetAuctionRequest, GetAuctionCommand>();
    }
}