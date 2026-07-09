using MediatR;

namespace MarketHub.Application.Features.Stores.Queries.GetStoreId;

public class GetStoreIdQuery : IRequest<Guid?>
{
    public Guid UserId { get; set; }
}