using System.Net;
using AutoMapper;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Brands.Commands.UpdateBrand;

public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, BaseResponse>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repositoryManager;
    public UpdateBrandCommandHandler(IMapper mapper, IRepositoryManager repositoryManager)
    {
        _mapper = mapper;
        _repositoryManager = repositoryManager;
    }

    public async Task<BaseResponse> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();
        UpdateBrandCommandValidator validator = new(_repositoryManager);

        ValidationResult validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Count > 0)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.ValidationErrors = new();

            foreach (ValidationFailure failure in validationResult.Errors)
                response.ValidationErrors.Add($"{failure.PropertyName},{failure.ErrorMessage}");

            return response;
        }

        Brand? brand = await _repositoryManager.BrandRepository.GetBrandByIdAsync(request.Id, trackChanges: true);

        if (brand is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"Brand with Id: {request.Id} not found.";

            return response;
        }

        _mapper.Map(request, brand);

        await _repositoryManager.SaveAsync();

        return response;
    }
}