using System.Text.Json;
using MarketHub.Application.Features.Orders.Commands.CancelOrder;
using MarketHub.Application.Features.Orders.Commands.Checkout;
using MarketHub.Application.Features.Orders.Commands.CreateOrder;
using MarketHub.Application.Features.Orders.Queries.GetOrder;
using MarketHub.Application.Features.Orders.Queries.GetOrders;
using MarketHub.Application.Responses;
using MarketHub.Application.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MarketHub.API.Controllers;

[ApiController]
[Route("api/users/{userId:guid?}/orders")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    public OrdersController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    [Route("{orderId:guid}")]
    public async Task<IActionResult> GetOrder(Guid userId, Guid orderId)
    {
        GetOrderQuery request = new()
        {
            UserId = userId,
            OrderId = orderId,
            TrackChanges = false
        };

        GetOrderQueryResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status404NotFound)
            return NotFound(response.Message);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return Ok(response.Order);
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders(Guid? userId, [FromBody] OrderParameters orderParameters)
    {
        orderParameters.UserId = userId;

        GetOrdersQuery request = new()
        {
            OrderParameters = orderParameters,
            TrackChanges = false
        };

        GetOrdersQueryResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(response.MetaData));

        return Ok(response.Orders);
    }

    [HttpPost]
    [Route("checkout")]
    public async Task<IActionResult> Checkout(Guid userId)
    {
        CheckoutCommand request = new() { UserId = userId };

        BaseResponse response = await _mediator.Send(request);

        if (!response.Success)
        {
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

            if (response.StatusCode == StatusCodes.Status409Conflict)
                return Conflict(response.Message);
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(Guid userId, [FromBody] CreateOrderCommand request)
    {
        request.UserId = userId;

        BaseResponse response = await _mediator.Send(request);

        if (!response.Success)
        {
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

            if (response.StatusCode == StatusCodes.Status409Conflict)
                return Conflict(response.Message);
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> CancelOrder(Guid userId, [FromBody] CancelOrderCommand request)
    {
        request.UserId = userId;

        BaseResponse response = await _mediator.Send(request);

        if (!response.Success)
        {
            if (response.StatusCode == StatusCodes.Status404NotFound)
                return NotFound(response.Message);

            if (response.StatusCode == StatusCodes.Status400BadRequest)
            {
                if (response.Message is not null)
                    return BadRequest(response.Message);

                foreach (string error in response.ValidationErrors!)
                {
                    string[] errorDetails = error.Split(',');
                    ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
                }

                return BadRequest(ModelState);
            }

            if (response.StatusCode == StatusCodes.Status409Conflict)
                return Conflict(response.Message);
        }

        return NoContent();
    }
}