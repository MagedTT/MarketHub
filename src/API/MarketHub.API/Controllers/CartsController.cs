using MarketHub.Application.Features.Carts.Commands.AddItemToCart;
using MarketHub.Application.Features.Carts.Commands.CreateCart;
using MarketHub.Application.Features.Carts.Commands.DeleteCart;
using MarketHub.Application.Features.Carts.Commands.RemoveCartItem;
using MarketHub.Application.Features.Carts.Commands.UpdateCartItemQuantity;
using MarketHub.Application.Features.Carts.Queries.GetCartByUserId;
using MarketHub.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MarketHub.API.Controllers;

[ApiController]
[Route("api/users/{userId:guid}/carts")]
public class CartsController : ControllerBase
{
    private readonly IMediator _mediator;
    public CartsController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetCart(Guid userId)
    {
        GetCartByUserIdQuery request = new() { UserId = userId };

        GetCartByUserIdQueryResponse response = await _mediator.Send(request);

        if (!response.Success)
        {
            // Change the behavior of this later so that if the cart for the user is null make a new one and return it.
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

        return Ok(response.Cart);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateCart(Guid userId)
    {
        CreateCartCommand request = new() { UserId = userId };

        CreateCartCommandResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return NoContent();
    }

    [HttpPost]
    [Route("AddCartItem/{productId:guid}")]
    public async Task<IActionResult> AddCartItem(Guid userId, Guid productId, int quantity)
    {
        AddItemToCartCommand request = new() { UserId = userId, ProductId = productId, Quantity = quantity };

        BaseResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return NoContent();
    }

    [HttpPut]
    [Route("updateCartItemQuantity")]
    public async Task<IActionResult> UpdateCartItemQuantity(Guid userId, [FromBody] UpdateCartItemQuantityCommand request)
    {
        request.UserId = userId;

        BaseResponse response = await _mediator.Send(request);

        if (response.StatusCode == StatusCodes.Status404NotFound)
            return NotFound(response.Message);

        if (response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return NoContent();
    }

    [HttpDelete]
    [Route("RemoveCartItem")]
    public async Task<IActionResult> RemoveCartItem(Guid userId, [FromBody] RemoveCartItemCommand request)
    {
        request.UserId = userId;

        BaseResponse response = await _mediator.Send(request);

        if (response.StatusCode == StatusCodes.Status404NotFound)
            return NotFound(response.Message);

        if (response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return NoContent();
    }

    [HttpDelete]
    [Route("{cartId:guid}")]
    public async Task<IActionResult> DeleteCart(Guid userId, Guid cartId)
    {
        DeleteCartCommand request = new() { UserId = userId, CartId = cartId };

        BaseResponse response = await _mediator.Send(request);

        if (!response.Success)
        {
            if (response.StatusCode == StatusCodes.Status404NotFound)
            {
                return NotFound(response.Message);
            }

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