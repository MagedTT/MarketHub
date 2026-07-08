
using MarketHub.Application.Features.PromoCodes.Commands.ActivatePromoCode;
using MarketHub.Application.Features.PromoCodes.Commands.CheckValidPromoCode;
using MarketHub.Application.Features.PromoCodes.Commands.CreatePromoCode;
using MarketHub.Application.Features.PromoCodes.Commands.DeactivatePromoCode;
using MarketHub.Application.Features.PromoCodes.Commands.DeletePromoCode;
using MarketHub.Application.Features.PromoCodes.Commands.ExtendPromoCodeValidity;
using MarketHub.Application.Features.PromoCodes.GetUsageCountForPromoCode;
using MarketHub.Application.Features.PromoCodes.GetUsageCountForPromoCodeByCode;
using MarketHub.Application.Features.PromoCodes.Queries.GetAllPromoCodes;
using MarketHub.Application.Features.PromoCodes.Queries.GetPromoCodeByCode;
using MarketHub.Application.Features.PromoCodes.Queries.GetPromoCodeById;
using MarketHub.Application.Responses;
using MarketHub.Application.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MarketHub.API.Controllers;

[ApiController]
[Route("api/promoCodes")]
public class PromoCodesController : ControllerBase
{
    private readonly IMediator _mediator;
    public PromoCodesController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost]
    [Route("all")]
    public async Task<IActionResult> GetAllPromoCodes([FromBody] PromoCodeParameters promoCodeParameters)
    {
        GetAllPromoCodesQuery request = new() { PromoCodeParameters = promoCodeParameters, TrackChanges = false };

        return Ok(await _mediator.Send(request));
    }

    [HttpGet]
    [Route("{promoCodeId:guid}")]
    public async Task<IActionResult> GetById(Guid promoCodeId)
    {
        GetPromoCodeByIdQuery request = new() { PromoCodeId = promoCodeId };

        GetPromoCodeByIdQueryResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status404NotFound)
            return NotFound(response.Message);

        return Ok(response.PromoCode);
    }

    [HttpGet]
    [Route("{code}")]
    public async Task<IActionResult> GetById(string code)
    {
        GetPromoCodeByCodeQuery request = new() { Code = code };

        GetPromoCodeByCodeQueryResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status404NotFound)
            return NotFound(response.Message);

        return Ok(response.PromoCode);
    }

    [HttpGet]
    [Route("usageCount/{promoCodeId:guid}")]
    public async Task<IActionResult> GetUsageCount(Guid promoCodeId)
    {
        GetUsageCountForPromoCodeQuery request = new() { PromoCodeId = promoCodeId };

        GetUsageCountForPromoCodeQueryResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return Ok(response.UsageCount);
    }

    [HttpGet]
    [Route("usageCount/{code}")]
    public async Task<IActionResult> GetUsageCountByCode(string code)
    {
        GetUsageCountForPromoCodeByCodeQuery request = new() { Code = code };

        GetUsageCountForPromoCodeByCodeQueryResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return Ok(response.UsageCount);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePromoCode([FromBody] CreatePromoCodeCommand request)
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

    [HttpPost]
    [Route("activate/{promoCodeId:guid}")]
    public async Task<IActionResult> ActivatePromoCode(Guid promoCodeId)
    {
        ActivatePromoCodeCommand request = new() { PromoCodeId = promoCodeId };

        BaseResponse response = await _mediator.Send(request);

        if (!response.Success)
        {
            if (response.StatusCode == StatusCodes.Status404NotFound)
                return NotFound(response.Message);

            else if (response.StatusCode == StatusCodes.Status400BadRequest)
                return BadRequest(response.Message);
        }

        return NoContent();
    }

    [HttpPost]
    [Route("checkValidity/{promoCodeId:guid}")]
    public async Task<IActionResult> CheckPromoCodeValidity(Guid promoCodeId)
    {
        CheckValidPromoCodeCommand request = new() { PromoCodeId = promoCodeId };

        BaseResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status404NotFound)
            return NotFound(response.Message);

        if (!response.Success && response.StatusCode == StatusCodes.Status406NotAcceptable)
            return BadRequest(response.Message);

        return Ok();
    }

    [HttpPost]
    [Route("deactivate/{promoCodeId:guid}")]
    public async Task<IActionResult> DeactivatePromoCode(Guid promoCodeId)
    {
        DeactivatePromoCodeCommand request = new() { PromoCodeId = promoCodeId };

        BaseResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status404NotFound)
            return NotFound(response.Message);

        return NoContent();
    }

    [HttpPost]
    [Route("extend")]
    public async Task<IActionResult> DeactivatePromoCode([FromBody] ExtendPromoCodeValidityCommand request)
    {
        BaseResponse response = await _mediator.Send(request);

        if (!response.Success)
        {
            if (response.StatusCode == StatusCodes.Status404NotFound)
                return NotFound(response.Message);

            else if (response.StatusCode == StatusCodes.Status406NotAcceptable)
                return BadRequest(response.Message);
        }

        return NoContent();
    }

    [HttpDelete]
    [Route("{promoCodeId:guid}")]
    public async Task<IActionResult> DeletePromoCode(Guid promoCodeId)
    {
        DeletePromoCodeCommand request = new() { PromoCodeId = promoCodeId };
        BaseResponse response = await _mediator.Send(request);

        if (!response.Success)
        {
            if (response.StatusCode == StatusCodes.Status404NotFound)
                return NotFound(response.Message);
            else if (response.StatusCode == StatusCodes.Status400BadRequest)
                return BadRequest(response.Message);
        }

        return NoContent();
    }
}