using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Stores.Queries.GetStore;

public class GetStoreQueryValidator : AbstractValidator<GetStoreQuery>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetStoreQueryValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.StoreId)
                .NotEmpty()
                .WithMessage("{PropertyName} is Required.");
    }
}