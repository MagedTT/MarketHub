using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Brands.Commands.UpdateBrand;

public class UpdateBrandCommand : IRequest<BaseResponse>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}