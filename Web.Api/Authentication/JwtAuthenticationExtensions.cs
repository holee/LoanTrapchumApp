using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Web.Api.Authentication;

public static class JwtAuthenticationExtensions
{
    /// <summary>
    /// Registers JWT bearer authentication for the API.
    /// Must be called AFTER <c>AddInfrastructureServices</c>, which registers the
    /// Identity cookie schemes: this call re-points the default authenticate/challenge
    /// schemes to bearer so API callers receive 401 responses instead of a login redirect.
    /// </summary>
    public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .Validate(o => !string.IsNullOrWhiteSpace(o.Issuer), "Jwt:Issuer must be configured.")
            .Validate(o => !string.IsNullOrWhiteSpace(o.Audience), "Jwt:Audience must be configured.")
            .Validate(o => !string.IsNullOrWhiteSpace(o.Key),
                "Jwt:Key must be configured. Set it with 'dotnet user-secrets set \"Jwt:Key\" \"<value>\"' or the Jwt__Key environment variable.")
            .Validate(o => Encoding.UTF8.GetByteCount(o.Key ?? string.Empty) >= 32,
                "Jwt:Key must be at least 32 bytes (256 bits) long for HMAC-SHA256.")
            .Validate(o => o.AccessTokenMinutes > 0, "Jwt:AccessTokenMinutes must be greater than zero.")
            .Validate(o => o.RefreshTokenDays > 0, "Jwt:RefreshTokenDays must be greater than zero.")
            .ValidateOnStart();

        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.MapInboundClaims = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtOptions.Audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    string.IsNullOrWhiteSpace(jwtOptions.Key) ? new string('0', 32) : jwtOptions.Key)),
                ClockSkew = TimeSpan.Zero,
                // Keep the long-form claim types the application already reads.
                NameClaimType = ClaimTypes.Name,
                RoleClaimType = ClaimTypes.Role
            };
        });

        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
