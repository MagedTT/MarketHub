namespace MarketHub.Application.Features.Products.Commands.AddProductCommand;

public class FileUploadDto
{
    public string FileExtension { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public int LengthInBytes { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public Stream Content { get; set; } = Stream.Null;
}