using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Orders.Commands.MarkOrderAsDelivered;

public class MarkOrderAsDeliveredCommand : IRequest<BaseResponse>
{
    public Guid OrderId { get; set; }
}