using MarketHub.Application.Contracts.Identity;
using MarketHub.Application.Models.Authentication;
using MarketHub.Application.Responses;
using MarketHub.Application.Responses.AuthenticationResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace MarketHub.API.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    public AuthenticationController(IAuthenticationService authenticationService)
        => _authenticationService = authenticationService;

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] UserForRegisterationDto request)
    {
        RegisterationResponse response = await _authenticationService.RegisterUserAsync(request);

        if (!response.Success)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                if (errorDetails.Length == 2)
                    ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
                else
                    ModelState.TryAddModelError("", errorDetails[0]);
            }

            return BadRequest(ModelState);
        }

        return Ok("Confirm your email");
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> AuthenticateUser([FromBody] UserForAuthenticationDto request)
    {
        BaseResponse validateResponse = await _authenticationService.ValidateUserAsync(request);

        if (!validateResponse.Success)
        {
            if (validateResponse.StatusCode == StatusCodes.Status404NotFound)
                return NotFound(validateResponse.Message);

            else if (validateResponse.StatusCode == StatusCodes.Status400BadRequest)
                return BadRequest(validateResponse.Message);

            else if (validateResponse.StatusCode == StatusCodes.Status401Unauthorized)
                return Unauthorized(validateResponse.Message);
        }

        TokenResponse tokenResponse = await _authenticationService.CreateTokenAsync(populateExp: true);

        if (!tokenResponse.Success && tokenResponse.StatusCode == StatusCodes.Status404NotFound)
            return NotFound(tokenResponse.Message);

        return Ok(tokenResponse.TokenDto);
    }

    [HttpGet]
    [Route("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string email, string token)
    {
        BaseResponse response = await _authenticationService.ConfirmEmail(email, token);

        if (!response.Success)
        {
            if (response.StatusCode == StatusCodes.Status404NotFound)
                return NotFound(response.Message);

            else if (response.StatusCode == StatusCodes.Status400BadRequest)
            {
                foreach (var error in response.ValidationErrors!)
                    ModelState.TryAddModelError("", error);
            }

            return BadRequest(ModelState);
        }

        return Ok($"{response.Success} - Confirmed");
    }
}