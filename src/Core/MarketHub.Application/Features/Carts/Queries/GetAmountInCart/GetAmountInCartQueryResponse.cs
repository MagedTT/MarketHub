using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Carts.Queries.GetAmountInCart;

public class GetAmountInCartQueryResponse : BaseResponse
{
    public int Amount { get; set; }

    public GetAmountInCartQueryResponse()
        : base()
    { }
}