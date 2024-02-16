using AutoMapper;
using Domain.Auction;
using Domain.Auction.Specification;
using FluentResults;
using MediatR;
using Shared.Domain.Interface;

namespace Application.UpdateBid;

public class PlaceBidCommandHandler:IRequestHandler<PlaceBidCommand, Result<AuctionDto>>
{
    private readonly IRepository<Auction> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PlaceBidCommandHandler(IRepository<Auction> repository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuctionDto>> Handle(PlaceBidCommand request, CancellationToken cancellationToken)
    {
        var spec = new AuctionByIdSpec(request.AuctionId);
        var auction = await this._repository.GetAsync(spec, cancellationToken);
        if (auction is null) return Result.Fail<AuctionDto>("auction not found");

        auction.PlaceBid(request.Status, request.BidAmount);
        await this._unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = this._mapper.Map<AuctionDto>(auction);
        return dto;
    }
}