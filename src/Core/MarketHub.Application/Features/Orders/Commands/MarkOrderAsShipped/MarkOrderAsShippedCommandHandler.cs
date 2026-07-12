using System.Net;
using AutoMapper;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Orders.Commands.MarkOrderAsShipped;

public class MarkOrderAsShippedCommandHandler : IRequestHandler<MarkOrderAsShippedCommand, BaseResponse>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repositoryManager;
    public MarkOrderAsShippedCommandHandler(IMapper mapper, IRepositoryManager repositoryManager)
    {
        _mapper = mapper;
        _repositoryManager = repositoryManager;
    }

    public async Task<BaseResponse> Handle(MarkOrderAsShippedCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();

        MarkOrderAsShippedCommandValidator validator = new(_repositoryManager);

        ValidationResult validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.ValidationErrors = new();

            foreach (ValidationFailure validationFailure in validationResult.Errors)
                response.ValidationErrors.Add($"{validationFailure.PropertyName},{validationFailure.ErrorMessage}");

            return response;
        }

        Order? order = await _repositoryManager.OrdersRepository.GetOrderByIdAsync(request.OrderId, true);

        if (order is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"Order with Id: {request.OrderId} is not found";

            return response;
        }

        if (order.Status != Domain.Enums.OrderStatus.Pending && order.Status != Domain.Enums.OrderStatus.Shipped)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = $"Can't mark as shipped";

            return response;
        }

        if (order.Status == Domain.Enums.OrderStatus.Shipped)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = $"Order is already shipped";

            return response;
        }

        order.Status = Domain.Enums.OrderStatus.Shipped;

        await _repositoryManager.SaveAsync();

        return response;
    }
}