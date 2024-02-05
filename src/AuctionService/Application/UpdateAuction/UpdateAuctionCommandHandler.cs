using AutoMapper;
using Domain.Auction;
using Domain.Auction.Specification;
using FluentResults;
using MediatR;
using Shared.Domain.Interface;

namespace Application.UpdateAuction;

public class UpdateAuctionCommandHandler : IRequestHandler<UpdateAuctionCommand, Result<AuctionDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepository<Auction> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAuctionCommandHandler(IMapper mapper, IRepository<Auction> repository, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<AuctionDto>> Handle(UpdateAuctionCommand request, CancellationToken cancellationToken)
    {
        var getByIdSpec = new AuctionByIdSpec(request.Id);
        var auction = await this._repository.GetAsync(getByIdSpec, cancellationToken);
        if (auction == null) return Result.Fail<AuctionDto>(new Error("auction not found."));

        if (!auction.Seller.Equals(request.Seller))
            return Result.Fail<AuctionDto>("invalid request. Auction seller is not the same.");

        auction.UpdateAuction(
            request.Make,
            request.Model,
            request.Color,
            request.Mileage,
            request.Year
        );

        await this._repository.UpdateAsync(auction, cancellationToken);
        await this._unitOfWork.SaveChangesAsync(cancellationToken);

        var result = this._mapper.Map<AuctionDto>(auction);
        return Result.Ok(result);
    }
}