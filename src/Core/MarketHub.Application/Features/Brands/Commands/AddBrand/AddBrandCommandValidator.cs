using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Brands.Commands.AddBrand;

public class AddBrandCommandValidator : AbstractValidator<AddBrandCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    public AddBrandCommandValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MaximumLength(100)
            .WithMessage("Brand Name length should be less than or equal to 100 characters.")
            .MustAsync(BeUniqueBrandName)
            .WithMessage("Brand Name already exists");
    }

    private async Task<bool> BeUniqueBrandName(string brandName, CancellationToken cancellationToken)
        => !await _repositoryManager.BrandRepository.BrandExistsByNameAsync(brandName);
}