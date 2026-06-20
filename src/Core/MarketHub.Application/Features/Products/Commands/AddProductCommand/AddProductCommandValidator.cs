using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Products.Commands.AddProductCommand;

public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
    private readonly IRepositoryManager _reopsitoryManager;
    public AddProductCommandValidator(IRepositoryManager repositoryManager)
    {
        _reopsitoryManager = repositoryManager;

        RuleFor(x => x.StoreId)
            .NotEmpty()
            .NotNull()
            .WithMessage("Store Id is Required")
            .MustAsync(StoreExists)
            .WithMessage($"Store does not exist.");

        RuleFor(x => x.BrandId)
            .NotEmpty()
            .NotNull()
            .WithMessage("Brand Id is Required")
            .MustAsync(BrandExists)
            .WithMessage("Brand does not exist");

        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage("Product Name is Required");

        RuleFor(x => x.Price)
            .NotEmpty()
            .NotNull()
            .WithMessage("Price is Required")
            .GreaterThan(0).WithMessage("Price should be greater than $0.0")
            .LessThanOrEqualTo(10_000).WithMessage("Price should be less that $10,000");

        RuleFor(x => x.Type)
            .NotEmpty()
            .NotNull()
            .WithMessage("Type is Required")
            .Must(type => new[] { "phones", "tvs", "electronics", "apparels" }.Contains(type.ToLower()))
            .WithMessage("Type must be phones, tvs, electronics, or apparels");

        RuleFor(x => x.Specifications)
            .NotEmpty()
            .NotNull()
            .WithMessage("Specifications is Required");

        RuleFor(x => x.AvailableQuantityInStock)
            .NotEmpty()
            .NotNull()
            .WithMessage("Avaiable Amount of Stock is Required")
            .GreaterThan(0)
            .WithMessage("Amount of stock should be greater that zero")
            .LessThan(1000)
            .WithMessage("Amount of stock should be less than 1000");

        RuleFor(x => x.Images)
            .NotEmpty()
            .NotNull()
            .WithMessage("Atleast 3 Images Should be uploadded");

        RuleForEach(x => x.Images).ChildRules(image =>
        {
            image.RuleFor(i => i.ContentType)
                .Must(contentType => contentType == "image/png" || contentType == "image/jpeg")
                .WithMessage("file type should be either image/jpeg or image/png");

            image.RuleFor(i => i.LengthInBytes)
                .LessThanOrEqualTo(5 * 1024 * 1024)
                .WithMessage("Image size in bytes must be less than 5 MB");
        });
    }

    private async Task<bool> StoreExists(Guid storeId, CancellationToken cancellationToken)
        => await _reopsitoryManager.StoreRepository.CheckStoreExistsAsync(storeId);


    private async Task<bool> BrandExists(Guid brandId, CancellationToken cancellationToken)
        => await _reopsitoryManager.BrandRepository.CheckBrandExistsAsync(brandId);
}

//     public Guid StoreId { get; set; }
//     public Guid BrandId { get; set; }
//     public string Name { get; set; } = string.Empty;
//     public string? Description { get; set; }
//     public double Price { get; set; }
//     public string Type { get; set; } = string.Empty;
//     public string Specifications { get; set; } = string.Empty;
//     public int AvailableQuantityInStock { get; set; }
//     public List<FileUploadDto> Images { get; set; } = new();