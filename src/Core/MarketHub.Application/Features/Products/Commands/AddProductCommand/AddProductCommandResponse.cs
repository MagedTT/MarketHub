using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Products.Commands.AddProductCommand;

public class AddProductCommandResponse : BaseResponse
{
    public Guid? CreatedProductId { get; set; }
    public AddProductCommandResponse()
        : base()
    { }
}