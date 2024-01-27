using AutoMapper;
using Domain.Auction;
using FluentResults;
using MassTransit;
using MediatR;
using Shared.Domain.Events;
using Shared.Domain.Interface;

namespace Application.CreateAuction;

public class CreateAuctionCommandHandler : IRequestHandler<CreateAuctionCommand, Result<AuctionDto>>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IRepository<Auction> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAuctionCommandHandler(IRepository<Auction> repository, IMapper mapper, IPublishEndpoint publishEndpoint, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuctionDto>> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
    {
        var auction = new Auction(
            Guid.NewGuid(),
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

        
        await this._repository.AddAsync(auction, cancellationToken);
        auction.RegisterDomainEvent(new AuctionCreated
        {
            Id = auction.Id,
            CreatedAt = auction.CreatedAt,
            UpdatedAt = auction.UpdatedAt,
            AuctionEnd = auction.AuctionEnd,
            Seller = auction.Seller,
            Winner = auction.Winner,
            Make = auction.Item.Make,
            Model = auction.Item.Model,
            Year = auction.Item.Year,
            Color = auction.Item.Color,
            Mileage = auction.Item.Mileage,
            ImageUrl = auction.Item.ImageUrl,
            Status = auction.AuctionStatus.Name,
            ReservePrice = auction.ReservePrice,
            SoldAmount = auction.SoldAmount,
            CurrentHighBid = auction.CurrentHighBid
        });

        await this._unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = this._mapper.Map<AuctionDto>(auction);
        return dto;
    }
}