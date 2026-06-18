using MarketHub.Application.DTOs.Persistence.Product;
using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Products.Queries.GetProductDetails;

public class GetProductDetailsQueryResponse : BaseResponse
{
    public ProductDetailsDto? ProductDetailsDto { get; set; }

    public GetProductDetailsQueryResponse()
        : base()
    { }
}