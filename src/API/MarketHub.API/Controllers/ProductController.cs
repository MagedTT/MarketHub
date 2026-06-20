using System.Text.Json;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Product;
using MarketHub.Application.Features.Products.Commands.AddProductCommand;
using MarketHub.Application.Features.Products.Queries.GetProductCard;
using MarketHub.Application.Features.Products.Queries.GetProductCards;
using MarketHub.Application.Features.Products.Queries.GetProductDetails;
using MarketHub.Application.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    [Route("productCard/{id:guid}")]
    public async Task<IActionResult> GetProductCard(Guid id)
    {
        GetProductCardQuery request = new()
        {
            ProductId = id,
            TrackChanges = false
        };

        GetProductCardQueryResponse response = await _mediator.Send(request);

        if (response.Success == false)
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
        }

        return Ok(response.ProductCardDto);
    }

    [HttpGet]
    [Route("productDetails/{id:guid}")]
    public async Task<IActionResult> GetProductDetails(Guid id)
    {
        GetProductDetailsQuery request = new()
        {
            ProductId = id,
            TrackChanges = false
        };

        GetProductDetailsQueryResponse response = await _mediator.Send(request);

        if (!response.Success)
        {
            if (response.StatusCode == StatusCodes.Status404NotFound)
                return NotFound(response.Message);

            else if (response.StatusCode == StatusCodes.Status400BadRequest)
            {
                foreach (string error in response.ValidationErrors!)
                {
                    string[] errorDetails = error.Split(",");
                    ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
                }

                return BadRequest(ModelState);
            }
        }

        return Ok(response.ProductDetailsDto);
    }

    [HttpGet]
    [Route("productCards")]
    public async Task<IActionResult> GetProductCards([FromBody] ProductParameters productParameters)
    {
        GetProductCardsQuery request = new()
        {
            TrackChanges = false,
            ProductParameters = productParameters
        };

        (IEnumerable<ProductCardDto> productCards, MetaData metaData) = await _mediator.Send(request);

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));

        return Ok(productCards);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddProductCommand request)
    {
        AddProductCommandResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return CreatedAtAction(nameof(GetProductDetails), new { id = response.CreatedProductId });
    }
}