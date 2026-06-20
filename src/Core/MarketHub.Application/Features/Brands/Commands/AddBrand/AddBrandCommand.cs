using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Brands.Commands.AddBrand;

public class AddBrandCommand : IRequest<BaseResponse>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}