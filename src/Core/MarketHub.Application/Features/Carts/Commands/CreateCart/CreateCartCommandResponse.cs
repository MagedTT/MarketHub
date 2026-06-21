using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Carts.Commands.CreateCart;

public class CreateCartCommandResponse : BaseResponse
{
    public Guid? CartId { get; set; }

    public CreateCartCommandResponse()
        : base()
    { }
}