using AutoMapper;
using Domain.Auction;
using Domain.Auction.Event;
using FluentResults;
using MediatR;
using Shared.Domain.Interface;

namespace Application.CreateAuction;

public class CreateAuctionCommandHandler : IRequestHandler<CreateAuctionCommand, Result<AuctionDto>>
{
    private readonly IRepository<Auction> _repository;
    private readonly IMapper _mapper;

    public CreateAuctionCommandHandler(IRepository<Auction> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<AuctionDto>> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
    {
        var auction = new Auction(
            request.ReservePrice,
            request.Seller,
            string.Empty,
            null,
            null,
            request.AuctionEnd
        );

        auction.AddItem(
            request.Make,
            request.Model,
            request.Year,
            request.Color,
            request.Mileage,
            request.ImageUrl
        );

        auction.RegisterDomainEvent(new AuctionCreated(
            auction.Id,
            auction.CreatedAt,
            auction.UpdatedAt,
            auction.AuctionEnd,
            auction.Seller,
            auction.Winner,
            auction.Item.Make,
            auction.Item.Model,
            auction.Item.Year,
            auction.Item.Color,
            auction.Item.Mileage,
            auction.Item.ImageUrl,
            auction.AuctionStatus.Name,
            auction.ReservePrice,
            auction.SoldAmount,
            auction.CurrentHighBid
            ));

        await this._repository.AddAsync(auction, cancellationToken);
        var dto = this._mapper.Map<AuctionDto>(auction);
        return dto;
    }
}