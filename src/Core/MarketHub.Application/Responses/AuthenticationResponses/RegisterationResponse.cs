using MarketHub.Application.Models.Authentication;
using Microsoft.AspNetCore.Identity;

namespace MarketHub.Application.Responses.AuthenticationResponses;

public class RegisterationResponse : BaseResponse
{
    public IdentityResult? IdentityResult { get; set; }

    public RegisterationResponse()
        : base()
    { }
}