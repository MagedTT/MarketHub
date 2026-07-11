using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQuery : IRequest<IEnumerable<Category>>
{
    public bool TrackChanges { get; set; }
}