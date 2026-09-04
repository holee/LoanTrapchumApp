using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Constants.Authorizations;
using Domains.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Web.Api.Authentication;

/// <summary>
/// Issues JWT access tokens whose claims match the shapes consumed by
/// <c>ClaimsPrincipalExtensions</c>, <c>CurrentUserRepository</c> and the
/// permission policies registered by <c>AppPolicyInjectionExtensions</c>.
/// </summary>
public sealed class JwtTokenService(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IOptions<JwtOptions> options) : IJwtTokenService
{
    private readonly JwtOptions _options = options.Value;
    private readonly JsonWebTokenHandler _handler = new();

    public async Task<AccessTokenResult> GenerateAccessTokenAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var roles = await userManager.GetRolesAsync(user);
        var userClaims = await userManager.GetClaimsAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (user.BranchRefId is not null)
        {
            claims.Add(new Claim(BranchIdClaimType, user.BranchRefId.Value.ToString()));
        }

        // Role claims plus the permission claims granted through each role.
        var permissions = new HashSet<string>(StringComparer.Ordinal);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));

            var applicationRole = await roleManager.FindByNameAsync(role);
            if (applicationRole is null) continue;

            foreach (var roleClaim in await roleManager.GetClaimsAsync(applicationRole))
            {
                if (roleClaim.Type == AppClaim.Permission)
                {
                    permissions.Add(roleClaim.Value);
                }
                else
                {
                    claims.Add(roleClaim);
                }
            }
        }

        // Permissions assigned directly to the user.
        foreach (var userClaim in userClaims)
        {
            if (userClaim.Type == AppClaim.Permission)
            {
                permissions.Add(userClaim.Value);
            }
            else
            {
                claims.Add(userClaim);
            }
        }

        claims.AddRange(permissions.Select(permission => new Claim(AppClaim.Permission, permission)));

        var issuedAt = DateTime.UtcNow;
        var expiresAt = issuedAt.AddMinutes(_options.AccessTokenMinutes);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            Subject = new ClaimsIdentity(claims),
            IssuedAt = issuedAt,
            NotBefore = issuedAt,
            Expires = expiresAt,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)),
                SecurityAlgorithms.HmacSha256)
        };

        return new AccessTokenResult(_handler.CreateToken(descriptor), expiresAt, roles.ToList());
    }

    public async Task<RefreshTokenResult> IssueRefreshTokenAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var expiresAt = DateTime.UtcNow.AddDays(_options.RefreshTokenDays);

        user.RefreshToken = token;
        user.RefreshTokenExpiryDate = expiresAt;
        await userManager.UpdateAsync(user);

        return new RefreshTokenResult(token, expiresAt);
    }

    public async Task RevokeRefreshTokenAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        user.RefreshToken = null;
        user.RefreshTokenExpiryDate = null;
        await userManager.UpdateAsync(user);
    }

    public Task<ApplicationUser?> FindByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return Task.FromResult<ApplicationUser?>(null);
        }

        return userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, cancellationToken);
    }

    /// <summary>Claim type read by <c>ClaimsPrincipalExtensions.GetBranchId</c>.</summary>
    internal const string BranchIdClaimType = "BranchId";
}
