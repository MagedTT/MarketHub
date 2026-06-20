using System.Net;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Brands.Queries.GetBrandById;

public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, GetBrandByIdQueryResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetBrandByIdQueryHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<GetBrandByIdQueryResponse> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
    {
        GetBrandByIdQueryResponse response = new();

        Brand? brand = await _repositoryManager.BrandRepository.GetBrandByIdAsync(request.BrandId, request.TrackChanges);

        if (brand is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"Brand with Id: {request.BrandId} is not found.";

            return response;
        }

        response.Brand = brand;

        return response;
    }
}