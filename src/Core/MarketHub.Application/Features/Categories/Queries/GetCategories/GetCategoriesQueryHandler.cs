using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<Category>>
{
    private IRepositoryManager _repositoryManager;
    public GetCategoriesQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<IEnumerable<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        => await _repositoryManager.CategoryRepository.GetCategoriesAsync(request.TrackChanges);
}