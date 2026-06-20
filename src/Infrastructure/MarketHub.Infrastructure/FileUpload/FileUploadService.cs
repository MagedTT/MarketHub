using MarketHub.Application.Contracts.Infrastructure;
using MarketHub.Application.Features.Products.Commands.AddProductCommand;

namespace MarketHub.Infrastructure.FileUpload;

public class FileUploadService : IFileUploadService
{
    public async Task<(bool status, string message, string filePath)> UploadFile(FileUploadDto file)
    {
        if (file is null)
            return await Task.FromResult((false, "File is null", ""));

        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

        Console.WriteLine($"======> {uploadsFolder}");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        string fileName = $"{Guid.NewGuid()}{file.FileExtension}";

        string filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
        {
            try
            {
                await file.Content.CopyToAsync(stream);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception while uploading file in file UploadFileService: ", exception.Message);
            }

            return await Task.FromResult((true, "", filePath));
        }
    }
}