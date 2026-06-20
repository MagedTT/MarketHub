using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Brands.Commands.DeleteBrand;

public class DeleteBrandCommand : IRequest<BaseResponse>
{
    public Guid BrandId { get; set; }
}