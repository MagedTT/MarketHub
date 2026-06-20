using System.Net;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Features.Brands.Commands.DeleteBrand;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, BaseResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public DeleteBrandCommandHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<BaseResponse> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();

        Brand? brand = await _repositoryManager.BrandRepository.GetBrandByIdAsync(request.BrandId, trackChanges: false);

        if (brand is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"Brand with Id: {request.BrandId} is not found";

            return response;
        }

        _repositoryManager.BrandRepository.DeleteBrand(brand);

        await _repositoryManager.SaveAsync();

        return response;
    }
}