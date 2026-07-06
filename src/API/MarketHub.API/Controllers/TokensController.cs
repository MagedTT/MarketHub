using MarketHub.Application.Contracts.Identity;
using MarketHub.Application.Models.Authentication;
using MarketHub.Application.Responses.AuthenticationResponses;
using Microsoft.AspNetCore.Mvc;

namespace MarketHub.API.Controllers;

[ApiController]
[Route("api/tokens")]
public class TokensController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    public TokensController(IAuthenticationService authenticationService)
        => _authenticationService = authenticationService;

    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenDto tokenDto)
    {
        TokenResponse response = await _authenticationService.RefreshTokenAsync(tokenDto);

        if (!response.Success && response.StatusCode == StatusCodes.Status404NotFound)
            return NotFound(response.Message);

        return Ok(response.TokenDto);
    }
}