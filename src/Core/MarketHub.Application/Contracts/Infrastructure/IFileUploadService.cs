using MarketHub.Application.Features.Products.Commands.AddProductCommand;

namespace MarketHub.Application.Contracts.Infrastructure;

public interface IFileUploadService
{
    Task<(bool status, string message, string filePath)> UploadFile(FileUploadDto file);
}