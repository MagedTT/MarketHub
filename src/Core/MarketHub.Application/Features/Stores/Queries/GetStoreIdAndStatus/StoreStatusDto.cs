namespace MarketHub.Application.Features.Stores.Queries.GetStoreIdAndStatus;

public class StoreStatusDto
{
    public Guid StoreId { get; set; }
    public bool IsActive { get; set; }
}