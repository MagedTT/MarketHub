using MarketHub.Application.Models.Authentication;
using Microsoft.AspNetCore.Identity;

namespace MarketHub.Application.Responses.AuthenticationResponses;

public class TokenResponse : BaseResponse
{
    public TokenDto? TokenDto { get; set; }

    public TokenResponse()
        : base()
    { }
}