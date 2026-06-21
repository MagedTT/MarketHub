using MarketHub.Application.DTOs.Persistence.Carts;
using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Carts.Queries.GetCartByUserId;

public class GetCartByUserIdQueryResponse : BaseResponse
{
    public CartDto? Cart { get; set; }

    public GetCartByUserIdQueryResponse()
        : base()
    { }
}