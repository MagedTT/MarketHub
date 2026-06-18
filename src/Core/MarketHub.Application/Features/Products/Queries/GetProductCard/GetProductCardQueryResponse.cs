using MarketHub.Application.DTOs.Persistence.Product;
using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Products.Queries.GetProductCard;

public class GetProductCardQueryResponse : BaseResponse
{
    public ProductCardDto? ProductCardDto { get; set; }

    public GetProductCardQueryResponse()
        : base()
    { }
}