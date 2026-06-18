using MarketHub.Application.DTOs.Persistence.Product;
using MarketHub.Application.Shared;
using MediatR;

namespace MarketHub.Application.Features.Products.Queries.GetProductCards;


public class GetProductCardsQuery : IRequest<(IEnumerable<ProductCardDto> productCards, MetaData metaData)>
{
    public bool TrackChanges { get; set; }
    public ProductParameters ProductParameters { get; set; } = new();
}