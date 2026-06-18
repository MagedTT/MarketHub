using MediatR;

namespace MarketHub.Application.Features.Products.Commands.AddProductCommand;

public class AddProductCommandHandler : IRequestHandler<AddProductCommand, AddProductCommandResponse>
{
    public Task<AddProductCommandResponse> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}