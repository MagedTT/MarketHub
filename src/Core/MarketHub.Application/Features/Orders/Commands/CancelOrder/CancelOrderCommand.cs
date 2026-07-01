using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Orders.Commands.CancelOrder;

public class CancelOrderCommand : IRequest<BaseResponse>
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
}