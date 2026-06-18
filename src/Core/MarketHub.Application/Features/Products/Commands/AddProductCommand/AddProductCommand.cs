using MediatR;

namespace MarketHub.Application.Features.Products.Commands.AddProductCommand;

public class AddProductCommand : IRequest<AddProductCommandResponse>
{
    public Guid StoreId { get; set; }
    public Guid BrandId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public double Price { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Specifications { get; set; } = string.Empty;
    public int AvailableQuantityInStock { get; set; }
    public List<FileUploadDto> Images { get; set; } = new();
}