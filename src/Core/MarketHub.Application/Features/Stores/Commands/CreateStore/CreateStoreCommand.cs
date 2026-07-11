using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Stores.Commands.CreateStore;

public class CreateStoreCommand : IRequest<BaseResponse>
{
    public Guid UserId { get; set; }
}