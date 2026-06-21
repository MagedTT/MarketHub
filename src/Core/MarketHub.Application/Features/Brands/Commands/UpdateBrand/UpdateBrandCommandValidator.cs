using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Brands.Commands.UpdateBrand;

public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    public UpdateBrandCommandValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MaximumLength(100)
            .WithMessage("Maximum number of characters is 100 character.");
        // .MustAsync(BeUniqueBrandNameAsync)
        // .WithMessage("Brand name already exists.");
    }

    private async Task<bool> BeUniqueBrandNameAsync(string brandName, CancellationToken cancellationToken)
        => !await _repositoryManager.BrandRepository.BrandExistsByNameAsync(brandName);
}