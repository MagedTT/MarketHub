using AutoMapper;
using FluentValidation;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Reviews.Commands.UpdateReview;

public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
    public UpdateReviewCommandValidator()
    {
        RuleFor(x => x.CurrentUserId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.");

        RuleFor(x => x.ReviewId)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.");

        RuleFor(x => x.Rating)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Rating should be between 0 and 5")
            .LessThanOrEqualTo(5)
            .WithMessage("Rating should be between 0 and 5");

        RuleFor(x => x.Comment)
            .MaximumLength(500)
            .WithMessage("Comment length should be less than 500 characters.");
    }
}