using Application.Features.Identity.Users;
using Domains.Models.Identity;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Authentication;

namespace Web.Api.Controllers.Identities;

[Route("api/auth")]
public class AuthController(
    UserManager<ApplicationUser> userManager,
    IJwtTokenService tokenService) : ApiControllerBase
{
    /// <summary>Exchanges username/email + password for a JWT access token and a refresh token.</summary>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email)
                   ?? await userManager.FindByNameAsync(request.Email);

        // Same message for unknown user and wrong password so accounts cannot be enumerated.
        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            return HandleResult<AuthResponse>(Error.Unauthorized(description: "Invalid credentials."));
        }

        if (user.IsActive == false)
        {
            return HandleResult<AuthResponse>(Error.Unauthorized(description: "Current user is not active."));
        }

        return HandleResult<AuthResponse>(await BuildResponseAsync(user, "Login Success", cancellationToken));
    }

    /// <summary>Rotates an expired access token using a still-valid refresh token.</summary>
    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return HandleResult<AuthResponse>(Error.Validation(description: "RefreshToken is required."));
        }

        var user = await tokenService.FindByRefreshTokenAsync(request.RefreshToken, cancellationToken);

        if (user is null || user.RefreshTokenExpiryDate is null || user.RefreshTokenExpiryDate <= DateTime.UtcNow)
        {
            return HandleResult<AuthResponse>(Error.Unauthorized(description: "Invalid or expired refresh token."));
        }

        if (user.IsActive == false)
        {
            return HandleResult<AuthResponse>(Error.Unauthorized(description: "Current user is not active."));
        }

        return HandleResult<AuthResponse>(await BuildResponseAsync(user, "Token refreshed", cancellationToken));
    }

    /// <summary>Invalidates the caller's refresh token.</summary>
    [Authorize]
    [HttpPost("revoke")]
    public async Task<IActionResult> Revoke([FromBody] RevokeTokenRequest request, CancellationToken cancellationToken)
    {
        var user = string.IsNullOrWhiteSpace(request.Token)
            ? await userManager.GetUserAsync(User)
            : await tokenService.FindByRefreshTokenAsync(request.Token, cancellationToken);

        if (user is null)
        {
            return HandleResult<string>(Error.NotFound(description: "No user found for the supplied token."));
        }

        await tokenService.RevokeRefreshTokenAsync(user, cancellationToken);
        return HandleResult<string>("Refresh token revoked.");
    }

    private async Task<AuthResponse> BuildResponseAsync(ApplicationUser user, string message, CancellationToken cancellationToken)
    {
        var accessToken = await tokenService.GenerateAccessTokenAsync(user, cancellationToken);
        var refreshToken = await tokenService.IssueRefreshTokenAsync(user, cancellationToken);

        return new AuthResponse
        {
            Message = message,
            IsAuthenticated = true,
            UserName = user.UserName,
            Email = user.Email,
            Role = accessToken.Roles.FirstOrDefault(),
            Roles = accessToken.Roles,
            BranchId = user.BranchRefId,
            Token = accessToken.Token,
            TokenExpiration = accessToken.ExpiresAtUtc,
            RefreshToken = refreshToken.Token,
            RefreshTokenExpiration = refreshToken.ExpiresAtUtc
        };
    }
}
