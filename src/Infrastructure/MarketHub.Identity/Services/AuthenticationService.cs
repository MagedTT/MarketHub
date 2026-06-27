using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Identity;
using MarketHub.Application.Contracts.Infrastructure;
using MarketHub.Application.FluentValidations.Identity;
using MarketHub.Application.Models.Authentication;
using MarketHub.Application.Models.Mail;
using MarketHub.Application.Responses;
using MarketHub.Application.Responses.AuthenticationResponses;
using MarketHub.Domain.Entities;
using MarketHub.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SendGrid;

namespace MarketHub.Identity.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly JwtSettings _jwtSettings;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private User? _user;

    public AuthenticationService(
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IOptionsMonitor<JwtSettings> jwtSettingsOptionsMonitor,
        IMapper mapper,
        IEmailService emailService
        )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtSettings = jwtSettingsOptionsMonitor.CurrentValue;
        _mapper = mapper;
        _emailService = emailService;
    }

    public async Task<RegisterationResponse> RegisterUserAsync(UserForRegisterationDto request)
    {
        RegisterationResponse response = new();

        UserForRegisterationDtoValidator validator = new();

        ValidationResult validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Count > 0)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.ValidationErrors = new();

            foreach (FluentValidation.Results.ValidationFailure failure in validationResult.Errors)
            {
                response.ValidationErrors.Add($"{failure.PropertyName},{failure.ErrorMessage}");
            }

            return response;
        }

        User user = _mapper.Map<User>(request);

        IdentityResult result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.ValidationErrors = new();

            foreach (var error in result.Errors)
                response.ValidationErrors.Add(error.Description);

            return response;
        }

        foreach (string role in request.Roles)
        {
            if (await _roleManager.RoleExistsAsync(role))
                await _userManager.AddToRoleAsync(user, role);
        }

        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        string encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        string confirmationEmail = $"https://localhost:5001/api/authentication/confirm-email?email={request.Email}&token={encodedToken}";

        Email email = new Email
        {
            ToName = $"{request.FirstName} {request.LastName}",
            ToEmail = request.Email,
            Subject = "MarketHub: Email Confirmation",
            Body = "Please Confirm Your Email",
            HtmlContent = confirmationEmail
            // HtmlContent = $"""
            //     <p>This is an email to confirm your email my bro.</p>
            //     <a href=${confirmationEmail}>Click Here</a> to Confirm Your Email and be able to login
            // """
        };

        Response emailResponse = await _emailService.SendEmailAsync(email);

        if (!emailResponse.IsSuccessStatusCode)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = "Failed to send Confirmation Email";

            return response;
        }

        response.IdentityResult = result;

        return response;
    }

    public async Task<BaseResponse> ConfirmEmail(string email, string encodedToken)
    {
        BaseResponse response = new();

        User? user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"User with Email: [{email}] not found.";

            return response;
        }

        string decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(encodedToken));

        IdentityResult result = await _userManager.ConfirmEmailAsync(user, decodedToken);

        if (!result.Succeeded)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = "Failed to confirm Email";

            response.ValidationErrors = new();

            foreach (IdentityError error in result.Errors)
                response.ValidationErrors.Add(error.Description);

            return response;
        }

        response.StatusCode = (int)HttpStatusCode.NoContent;

        return response;
    }

    public async Task<BaseResponse> ValidateUserAsync(UserForAuthenticationDto request)
    {
        BaseResponse response = new();

        _user = await _userManager.FindByEmailAsync(request.Email);

        if (_user is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = "User not found";

            return response;
        }

        if (!await _userManager.IsEmailConfirmedAsync(_user))
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = "Email not confirmed.";

            return response;
        }

        if (!await _userManager.CheckPasswordAsync(_user!, request.Password))
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.Unauthorized;
            response.Message = "You are not authorized";
        }

        return response;
    }

    public async Task<TokenResponse> CreateTokenAsync(bool populateExp)
    {
        TokenResponse response = new();

        if (_user is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = "User not found.";

            return response;
        }

        SigningCredentials signingCredentials = GetSigningCredentials();
        List<Claim> claims = await GetClaims();

        JwtSecurityToken tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        string accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        string refreshToken = GenerateRefreshToken();

        _user.RefreshToken = refreshToken;

        if (populateExp)
            _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        await _userManager.UpdateAsync(_user);

        response.TokenDto = new TokenDto(accessToken, refreshToken);

        return response;
    }

    public async Task<TokenResponse> RefreshTokenAsync(TokenDto tokenDto)
    {
        TokenResponse response = new();

        ClaimsPrincipal claimsPrincipal = GetClaimsPrincipalFromExpiresAccessToken(tokenDto.AccessToken);

        User? user = await _userManager.FindByIdAsync(claimsPrincipal.FindFirstValue("sub") ?? "");

        if (user is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = "User not found.";

            return response;
        }

        if (user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            throw new Exception("Invalid Old Refresh Token");

        _user = user;

        return await CreateTokenAsync(populateExp: false);
    }

    private SigningCredentials GetSigningCredentials()
    {
        byte[] key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("MarketHub__SigningKey")!);

        SymmetricSecurityKey symmetricSecurityKey = new(key);

        SigningCredentials signingCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        return signingCredentials;
    }

    private async Task<List<Claim>> GetClaims()
    {
        List<Claim> claims = new()
        {
            new Claim(JwtRegisteredClaimNames.Name, _user?.UserName!),
            new Claim(JwtRegisteredClaimNames.Sub, _user?.Id.ToString()!)
        };

        IList<string> roles = await _userManager.GetRolesAsync(_user ?? new());

        foreach (string role in roles)
            claims.Add(new Claim(role, role));

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        JwtSecurityToken tokenOptions = new JwtSecurityToken(
            issuer: _jwtSettings.ValidIssuer,
            audience: _jwtSettings.ValidAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.ExpiresAt)),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }

    private string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];

        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }

    private ClaimsPrincipal GetClaimsPrincipalFromExpiresAccessToken(string token)
    {
        TokenValidationParameters tokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = _jwtSettings.ValidIssuer,
            ValidAudience = _jwtSettings.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("MarketHub__SigningKey")!))
        };

        JwtSecurityTokenHandler tokenHandler = new();

        SecurityToken securityToken;

        ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

        JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new Exception("Invalid Token");

        return claimsPrincipal;
    }
}