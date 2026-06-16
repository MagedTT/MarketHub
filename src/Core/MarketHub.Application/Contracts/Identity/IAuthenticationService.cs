using MarketHub.Application.Models.Authentication;
using MarketHub.Application.Responses;
using MarketHub.Application.Responses.AuthenticationResponses;
using Microsoft.AspNetCore.Identity;

namespace MarketHub.Application.Contracts.Identity;

public interface IAuthenticationService
{
    Task<RegisterationResponse> RegisterUserAsync(UserForRegisterationDto request);
    Task<BaseResponse> ValidateUserAsync(UserForAuthenticationDto request);
    Task<TokenResponse> CreateTokenAsync(bool populateExp);
    Task<TokenResponse> RefreshTokenAsync(TokenDto tokenDto);
    Task<BaseResponse> ConfirmEmail(string email, string encodedToken);
}