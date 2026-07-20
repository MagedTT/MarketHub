using MediatR;

namespace MarketHub.Application.Features.Stores.Queries.GetStoreIdAndStatus;

public class GetStoreIdAnsActiveStatusQuery : IRequest<StoreStatusDto?>
{
    public Guid UserId { get; set; }
}