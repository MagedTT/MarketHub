using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Features.Brands.Queries.GetBrandById;

public class GetBrandByIdQueryResponse : BaseResponse
{
    public Brand? Brand { get; set; }
    public GetBrandByIdQueryResponse()
        : base()
    { }
}