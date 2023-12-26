using AutoMapper;
using Domain.Auction;
using Domain.Auction.Specification;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interface;

namespace Application.SearchAuction;

public class SearchAuctionCommandHandler : IRequestHandler<SearchAuctionCommand, Result<List<AuctionDto>>>
{
    private readonly IRepository<Auction> _repository;
    private readonly ILogger<SearchAuctionCommandHandler> _logger;
    private readonly IMapper _mapper;

    public SearchAuctionCommandHandler(IRepository<Auction> repository, ILogger<SearchAuctionCommandHandler> logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<List<AuctionDto>>> Handle(SearchAuctionCommand request, CancellationToken cancellationToken)
    {
        var spec = new AuctionSearchSpec(request.From);
        var result = (await this._repository.ListAsync(spec, cancellationToken))
            .Select(this._mapper.Map<AuctionDto>)
            .ToList();

        return result;
    }
}