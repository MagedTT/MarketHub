using System.Net;
using AutoMapper;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Wishlist;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Wishlists.Queries.GetWishlist;

public class GetWishlistQueryHandler : IRequestHandler<GetWishlistQuery, GetWishlistQueryResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    public GetWishlistQueryHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<GetWishlistQueryResponse> Handle(GetWishlistQuery request, CancellationToken cancellationToken)
    {
        GetWishlistQueryResponse response = new();

        GetWishlistQueryValidator validator = new(_repositoryManager);

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

        WishlistDto wishlistDto;

        bool wishlistExists = await _repositoryManager.WishlistRepository.CheckWishlistExistsByUserIdAsync(request.UserId);

        if (wishlistExists)
            wishlistDto = await _repositoryManager.WishlistRepository.GetWishlistWithItemsByUserIdAsync(request.UserId, request.TrackChanges);

        else
        {
            Wishlist wishlist = new()
            {
                UserId = request.UserId
            };

            _repositoryManager.WishlistRepository.CreateWishlist(wishlist);

            wishlistDto = _mapper.Map<WishlistDto>(wishlist);

            await _repositoryManager.SaveAsync();
        }

        response.Wishlist = wishlistDto;

        return response;
    }
}