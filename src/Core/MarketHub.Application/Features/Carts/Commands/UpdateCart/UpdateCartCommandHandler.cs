// using System.Net;
// using FluentValidation.Results;
// using MarketHub.Application.Contracts.Persistence;
// using MarketHub.Application.Responses;
// using MediatR;

// namespace MarketHub.Application.Features.Carts.Commands.UpdateCart;

// public class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand, BaseResponse>
// {
//     private readonly IRepositoryManager _repositoryManager;
//     public UpdateCartCommandHandler(IRepositoryManager repositoryManager)
//         => _repositoryManager = repositoryManager;

//     public async Task<BaseResponse> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
//     {
//         BaseResponse response = new();
//         UpdateCartCommandValidator validator = new();

//         ValidationResult validationResult = await validator.ValidateAsync(request);

//         if (validationResult.Errors.Count > 0)
//         {
//             response.Success = false;
//             response.StatusCode = (int)HttpStatusCode.NotFound;
//             response.Message = validationResult.Errors[0].ErrorMessage;

//             return response;
//         }


//     }
// }