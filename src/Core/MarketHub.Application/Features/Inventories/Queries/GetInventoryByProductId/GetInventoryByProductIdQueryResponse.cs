using MarketHub.Application.DTOs.Persistence.Inventories;
using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Inventories.Queries.GetInventoryByProductId;

public class GetInventoryByProductIdQueryResponse : BaseResponse
{
    public InventoryDto? Inventory { get; set; }
    public GetInventoryByProductIdQueryResponse()
        : base()
    { }
}