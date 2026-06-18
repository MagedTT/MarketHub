using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Features.Products.Commands.AddProductCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/Temp")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<IActionResult> Create(AddProductCommand request)
    {
        AddProductCommandResponse response = await _mediator.Send(request);

        return Ok(response);
    }
}