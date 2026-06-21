using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Carts.Queries.GetCartByUserId;

public class GetCartByUserIdQueryValidator : AbstractValidator<GetCartByUserIdQuery>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetCartByUserIdQueryValidator(IRepositoryManager repositoryManager)
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