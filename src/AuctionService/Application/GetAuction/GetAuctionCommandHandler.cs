using AutoMapper;
using Domain.Auction;
using Domain.Auction.Specification;
using FluentResults;
using MediatR;
using Shared.Domain.Interface;

namespace Application.GetAuction;

public class GetAuctionCommandHandler : IRequestHandler<GetAuctionCommand, Result<AuctionDto>>
{
    private readonly IRepository<Auction> _repository;
    private readonly IMapper _mapper;

    public GetAuctionCommandHandler(IRepository<Auction> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<Result<AuctionDto>> Handle(GetAuctionCommand request, CancellationToken cancellationToken)
    {
        var spec = new AuctionByIdSpec(request.Id);
        var auction = await this._repository.GetAsync(spec, cancellationToken);
        if (auction == null) return Result.Fail<AuctionDto>("auction not found");

        var result = this._mapper.Map<AuctionDto>(auction);
        return result;
    }
}