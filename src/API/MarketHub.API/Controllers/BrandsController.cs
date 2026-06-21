using System.Text.Json;
using MarketHub.Application.Features.Brands.Commands.AddBrand;
using MarketHub.Application.Features.Brands.Commands.DeleteBrand;
using MarketHub.Application.Features.Brands.Commands.UpdateBrand;
using MarketHub.Application.Features.Brands.Queries.GetBrandById;
using MarketHub.Application.Features.Brands.Queries.GetBrandsList;
using MarketHub.Application.Features.Brands.Queries.GetBrandsNames;
using MarketHub.Application.Responses;
using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MarketHub.API.Controllers;

[ApiController]
[Route("api/brands")]
public class BrandsController : ControllerBase
{
    private readonly IMediator _mediator;
    public BrandsController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetBrandById(Guid id)
    {
        GetBrandByIdQuery request = new() { BrandId = id, TrackChanges = false };

        GetBrandByIdQueryResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status404NotFound)
            return NotFound(response.Message);

        return Ok(response.Brand);
    }

    [HttpGet]
    public async Task<IActionResult> GetBrandsList([FromBody] BrandParameters parameters)
    {
        GetBrandsQuery request = new() { TrackChanges = false, BrandParameters = parameters };

        (IEnumerable<Brand> brands, MetaData metaData) = await _mediator.Send(request);

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));

        return Ok(brands);
    }

    [HttpGet]
    [Route("names")]
    public async Task<IActionResult> GetBrandsNames([FromBody] BrandParameters parameters)
    {
        GetBrandsNamesQuery request = new() { TrackChanges = false, BrandParameters = parameters };

        (IEnumerable<string> brandsNames, MetaData metaData) = await _mediator.Send(request);

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));

        return Ok(brandsNames);
    }

    [HttpPost]
    public async Task<IActionResult> AddBrand([FromBody] AddBrandCommand request)
    {
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
    public async Task<IActionResult> UpdateBrand([FromBody] UpdateBrandCommand request)
    {
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
        }

        return NoContent();
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteBrand(Guid id)
    {
        DeleteBrandCommand request = new() { BrandId = id };

        BaseResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status404NotFound)
        {
            return NotFound(response.Message);
        }

        return NoContent();
    }
}