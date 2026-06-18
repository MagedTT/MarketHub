using FluentValidation;

namespace MarketHub.Application.Features.Products.Commands.AddProductCommand;

public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
    public AddProductCommandValidator()
    {

    }
}