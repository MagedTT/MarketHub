using FluentValidation;
using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Carts.Queries.GetAmountInCart;

public class GetAmountInCartQueryValidator : AbstractValidator<GetAmountInCartQuery>
{
    private readonly IRepositoryManager _repositoryManager;

    public GetAmountInCartQueryValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is Required.")
            .MustAsync(UserExists)
            .WithMessage("User with this Id is not found.");
    }

    private async Task<bool> UserExists(Guid userId, CancellationToken cancellationToken)
        => await _repositoryManager.UserRepository.CheckUserExistsAsync(userId);
}