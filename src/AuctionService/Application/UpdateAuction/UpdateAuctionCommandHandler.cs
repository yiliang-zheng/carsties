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

    public UpdateAuctionCommandHandler(IMapper mapper, IRepository<Auction> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }
    public async Task<Result<AuctionDto>> Handle(UpdateAuctionCommand request, CancellationToken cancellationToken)
    {
        var getByIdSpec = new AuctionByIdSpec(request.Id);
        var auction = await this._repository.GetAsync(getByIdSpec, cancellationToken);
        if (auction == null) return Result.Fail<AuctionDto>(new Error("auction not found."));

        auction.UpdateAuction(
            request.Make,
            request.Model,
            request.Color,
            request.Mileage,
            request.Year
        );

        await this._repository.UpdateAsync(auction, cancellationToken);

        var result = this._mapper.Map<AuctionDto>(auction);
        return Result.Ok(result);
    }
}