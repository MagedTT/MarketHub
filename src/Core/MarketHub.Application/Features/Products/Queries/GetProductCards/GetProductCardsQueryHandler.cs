using AutoMapper;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Product;
using MarketHub.Application.Shared;
using MediatR;

namespace MarketHub.Application.Features.Products.Queries.GetProductCards;

public class GetProductCardsQueryHandler : IRequestHandler<GetProductCardsQuery, (IEnumerable<ProductCardDto> productCards, MetaData metaData)>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetProductCardsQueryHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<(IEnumerable<ProductCardDto> productCards, MetaData metaData)> Handle(GetProductCardsQuery request, CancellationToken cancellationToken)
    {
        PagedList<ProductCardDto> pagedProductCards = await _repositoryManager.ProductRepository.GetProductCardsAsync(request.ProductParameters, request.TrackChanges);

        return (productCards: pagedProductCards.AsEnumerable(), metaData: pagedProductCards.MetaData);
    }
}