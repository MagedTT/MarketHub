using MarketHub.Application.Features.Products.Commands.AddProductCommand;

namespace MarketHub.Application.Contracts.Infrastructure;

public interface IFileUploadService
{
    Task<string> UploadFile(FileUploadDto file);
}