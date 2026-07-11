using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Products.Commands.DeactivateProductCommand;

public class ActivateProductCommand : IRequest<BaseResponse>
{
    public Guid StoreId { get; set; }
    public Guid ProductId { get; set; }
}