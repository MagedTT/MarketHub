using MarketHub.Application.Features.Wishlists.Commands.AddWishlistItem;
using MarketHub.Application.Features.Wishlists.Commands.RemoveWishlistItem;
using MarketHub.Application.Features.Wishlists.Queries.GetWishlist;
using MarketHub.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MarketHub.API.Controllers;

[ApiController]
[Route("api/users/{userId:guid}/wishlists")]
public class WishlistController : ControllerBase
{
    private readonly IMediator _mediator;
    public WishlistController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetWishlist(Guid userId)
    {
        GetWishlistQuery request = new()
        {
            UserId = userId,
            TrackChanges = false
        };

        GetWishlistQueryResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return Ok(response.Wishlist);
    }

    [HttpPost]
    public async Task<IActionResult> AddWishlistItem(Guid userId, [FromBody] AddWishlistItemCommand request)
    {
        request.UserId = userId;

        BaseResponse response = await _mediator.Send(request);

        if (!response.Success)
        {
            if (response.StatusCode == StatusCodes.Status406NotAcceptable)
                return BadRequest(response.Message);

            else if (response.StatusCode == StatusCodes.Status400BadRequest)
            {
                foreach (string error in response.ValidationErrors!)
                {
                    string[] errorDetails = error.Split(',');
                    ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
                }

                return BadRequest(ModelState);
            }

            return BadRequest();
        }

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteWishlistItem(Guid userId, [FromBody] RemoveWishlistItemCommand request)
    {
        request.UserId = userId;

        BaseResponse response = await _mediator.Send(request);

        if (!response.Success)
        {
            if (response.StatusCode == StatusCodes.Status404NotFound)
                return NotFound(response.Message);

            else if (response.StatusCode == StatusCodes.Status400BadRequest)
            {
                foreach (string error in response.ValidationErrors!)
                {
                    string[] errorDetails = error.Split(',');
                    ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
                }

                return BadRequest(ModelState);
            }

            return BadRequest();
        }

        return NoContent();
    }
}