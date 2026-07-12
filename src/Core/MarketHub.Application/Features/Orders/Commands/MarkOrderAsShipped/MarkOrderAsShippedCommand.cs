using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Orders.Commands.MarkOrderAsShipped;

public class MarkOrderAsShippedCommand : IRequest<BaseResponse>
{
    public Guid OrderId { get; set; }
}