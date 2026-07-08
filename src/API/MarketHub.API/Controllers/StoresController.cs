using System.Text.Json;
using MarketHub.Application.Features.Brands.Queries.GetTotalBrandsForStore;
using MarketHub.Application.Features.Orders.Queries.GetOrderStatusCountForStore;
using MarketHub.Application.Features.Orders.Queries.GetRecentOrdersForStore;
using MarketHub.Application.Features.Orders.Queries.GetTotalOrdersForStore;
using MarketHub.Application.Features.Products.GetTopNBestSellingProductsForStore;
using MarketHub.Application.Features.Products.Queries.GetAllProductsForStore;
using MarketHub.Application.Features.Products.Queries.GetTotalProductsForStore;
using MarketHub.Application.Features.Products.Queries.GetTotalProductsInStockForStore;
using MarketHub.Application.Features.Products.Queries.GetTotalProductsOutOfStockForStore;
using MarketHub.Application.Features.PromoCodes.Queries.GetTotalPromoCodesForStore;
using MarketHub.Application.Features.Reviews.Queries.GetRatingCountForStore;
using MarketHub.Application.Features.Reviews.Queries.GetTotalReviewsForStore;
using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MarketHub.API.Controllers;

[ApiController]
[Route("api/stores")]
public class StoresController : ControllerBase
{
    private readonly IMediator _mediator;
    public StoresController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    [Route("totalBrands/{storeId:guid}")]
    public async Task<IActionResult> GetTotalBrandsCount(Guid storeId)
    {
        GetTotalBrandsForStoreQuery request = new() { StoreId = storeId };

        return Ok(await _mediator.Send(request));
    }

    [HttpGet]
    [Route("top/{n:int}/sellingBrands/{storeId:guid}")]
    public async Task<IActionResult> GetTopNSellingBrands(Guid storeId, int n)
    {
        GetTopNSellingBrandsForStoreQuery request = new() { StoreId = storeId, N = n };

        GetTopNSellingBrandsForStoreQueryResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return Ok(response.Brands);
    }

    [HttpGet]
    [Route("totalOrders/{storeId:giud}")]
    public async Task<IActionResult> GetTotalOrders(Guid storeId)
    {
        GetTotalOrdersForStoreQuery request = new() { StoreId = storeId };

        GetTotalOrdersForStoreQueryResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return Ok(response.TotalOrders);
    }

    [HttpGet]
    [Route("OrderStatusCountByStore/{storeId:giud}")]
    public async Task<IActionResult> GetOrderStatusCount(Guid storeId)
    {
        GetOrderStatusCountForStoreQuery request = new() { StoreId = storeId };

        GetOrderStatusCountForStoreQueryResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return Ok(response.StoreOrderStatusCounts);
    }

    [HttpGet]
    [Route("RecentOrders/{storeId:guid}")]
    public async Task<IActionResult> GetRecentOrders(Guid storeId, [FromBody] StoreOrdersParameters storeOrdersParameters)
    {
        GetRecentOrdersForStoreCommand request = new() { StoreId = storeId, StoreOrdersParameters = storeOrdersParameters };

        GetRecentOrdersForStoreCommandResponse response = await _mediator.Send(request);

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

    [HttpGet]
    [Route("TotalProducts/{storeId:guid}")]
    public async Task<IActionResult> GetTotalProducts(Guid storeId)
    {
        GetTotalProductsForStoreQuery request = new() { StoreId = storeId };

        GetTotalProductsForStoreQueryResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return Ok(response.TotalProducts);
    }

    [HttpGet]
    [Route("TotalProductsInStock/{storeId:guid}")]
    public async Task<IActionResult> GetTotalProductsInStock(Guid storeId)
    {
        GetTotalProductsInStockForStoreQuery request = new() { StoreId = storeId };

        GetTotalProductsInStockForStoreQueryResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return Ok(response.TotalProductsInStock);
    }

    [HttpGet]
    [Route("TotalProductsOutOfStock/{storeId:guid}")]
    public async Task<IActionResult> GetTotalProductsOutOfStock(Guid storeId)
    {
        GetTotalProductsOutOfStockForStoreQuery request = new() { StoreId = storeId };

        GetTotalProductsOutOfStockForStoreQueryResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return Ok(response.TotalProductsOutOfStock);
    }

    [HttpGet]
    [Route("AllProducts/{storeId:guid}")]
    public async Task<IActionResult> GetTotalProductsOutOfStock(Guid storeId, [FromBody] StoreProductsParameters storeProductsParameters)
    {
        GetAllProductsForStoreQuery request = new() { StoreId = storeId, StoreProductParameters = storeProductsParameters };

        GetAllProductsForStoreQueryResponse response = await _mediator.Send(request);

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

        return Ok(response.Products);
    }

    [HttpGet]
    [Route("Top/{n:int}/sellnigProducts/{storeId:guid}")]
    public async Task<IActionResult> GetTopNSellingProducts(Guid storeId, int n)
    {
        GetTopNBestSellingProductsForStoreQuery request = new() { StoreId = storeId, N = n };

        GetTopNBestSellingProductsForStoreQueryResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return Ok(response.Products);
    }

    [HttpGet]
    [Route("TotalReviews/{storeId:guid}")]
    public async Task<IActionResult> GetTotalReviews(Guid storeId)
    {
        GetTotalReviewsForStoreQuery request = new() { StoreId = storeId };

        GetTotalReviewsForStoreQueryResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return Ok(response.TotalReviews);
    }

    [HttpGet]
    [Route("RatingCountByStore/{storeId:guid}")]
    public async Task<IActionResult> GetRatingCount(Guid storeId)
    {
        GetRatingCountForStoreQuery request = new() { StoreId = storeId };

        GetRatingCountForStoreQueryResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return Ok(response.StoreRatingCount);
    }

    [HttpGet]
    [Route("totalPromoCodes/{storeId:guid}")]
    public async Task<IActionResult> GetTotalPromoCodes(Guid storeId)
    {
        GetTotalPromoCodesForStoreQuery request = new() { StoreId = storeId };

        GetTotalPromoCodesForStoreQueryResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return Ok(response.TotalPromoCodes);
    }
}