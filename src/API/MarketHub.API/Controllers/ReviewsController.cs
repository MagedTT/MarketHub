using System.Security.Claims;
using System.Text.Json;
using AutoMapper;
using MarketHub.Application.DTOs.Persistence.Review;
using MarketHub.Application.Features.Reviews.Commands.CreateReview;
using MarketHub.Application.Features.Reviews.Commands.DeleteReview;
using MarketHub.Application.Features.Reviews.Commands.UpdateReview;
using MarketHub.Application.Features.Reviews.Queries.GetReviewByUserId;
using MarketHub.Application.Features.Reviews.Queries.GetReviewsList;
using MarketHub.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketHub.API.Controllers;

[ApiController]
[Route("api/reviews")]
[Authorize]
public class ReviewsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    public ReviewsController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet]
    [Route("review")]
    [AllowAnonymous]
    public async Task<IActionResult> GetReview([FromBody] GetReviewByUserIdAndProductIdQuery request)
    {
        GetReviewByUserIdAndProductIdQueryResponse response = await _mediator.Send(request);

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

        return Ok(response.Review);
    }

    [HttpGet]
    [Route("reviews")]
    [AllowAnonymous]
    public async Task<IActionResult> GetReviews([FromBody] GetReviewsListQuery request)
    {
        GetReviewsListQueryResponse response = await _mediator.Send(request);

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

        return Ok(response.Reviews);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewCommand request)
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
    [Route("{reviewId:guid}")]
    public async Task<IActionResult> UpdateReview(Guid reviewId, [FromBody] UpdateReviewRequest updateReviewRequest)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
            return Unauthorized();

        UpdateReviewCommand request = _mapper.Map<UpdateReviewCommand>(updateReviewRequest);

        request.CurrentUserId = userId;
        request.ReviewId = reviewId;

        BaseResponse response = await _mediator.Send(request);

        if (!response.Success)
        {
            if (response.StatusCode == StatusCodes.Status404NotFound)
                return NotFound(response.Message);

            else if (response.StatusCode == StatusCodes.Status403Forbidden)
                return Forbid();

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
    [Route("{reviewId:guid}")]
    public async Task<IActionResult> DeleteReview(Guid reviewId)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
            return Unauthorized();

        DeleteReviewCommand request = new()
        {
            ReviewId = reviewId,
            CurrentUserId = userId
        };

        BaseResponse response = await _mediator.Send(request);

        if (!response.Success)
        {
            if (response.StatusCode == StatusCodes.Status404NotFound)
                return NotFound(response.Message);

            else if (response.StatusCode == StatusCodes.Status403Forbidden)
                return Forbid();

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