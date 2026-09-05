using System.Security.Claims;
using System.Text.Json;
using Applications.Constants.Authorizations;

namespace Web.UI.Services.Authentications;

/// <summary>Reads the claims out of a JWT payload without validating the signature.</summary>
/// <remarks>
/// The client only needs the claims to drive UI gating; the API re-validates every request.
/// </remarks>
public static class JwtClaimsParser
{
    /// <summary>Short JWT claim names mapped back to the long types the application policies expect.</summary>
    private static readonly Dictionary<string, string> ShortNameMap = new(StringComparer.Ordinal)
    {
        ["nameid"] = ClaimTypes.NameIdentifier,
        ["unique_name"] = ClaimTypes.Name,
        ["email"] = ClaimTypes.Email,
        ["role"] = ClaimTypes.Role,
        ["roles"] = ClaimTypes.Role,
        ["mobilephone"] = ClaimTypes.MobilePhone
    };

    public static IReadOnlyList<Claim> Parse(string? accessToken)
    {
        var payload = ReadPayload(accessToken);

        if (payload is null) return [];

        var claims = new List<Claim>();

        foreach (var property in payload.RootElement.EnumerateObject())
        {
            var type = ShortNameMap.TryGetValue(property.Name, out var mapped) ? mapped : property.Name;

            if (property.Value.ValueKind == JsonValueKind.Array)
            {
                claims.AddRange(property.Value.EnumerateArray()
                    .Select(element => new Claim(type, element.ToString())));
            }
            else
            {
                claims.Add(new Claim(type, property.Value.ToString()));
            }
        }

        return claims;
    }

    /// <summary>Expiry encoded in the token's <c>exp</c> claim, or null when absent/unparsable.</summary>
    public static DateTimeOffset? GetExpiry(string? accessToken)
    {
        var payload = ReadPayload(accessToken);

        if (payload is null) return null;

        if (!payload.RootElement.TryGetProperty(AppClaim.Expiration, out var exp)) return null;

        return exp.ValueKind switch
        {
            JsonValueKind.Number when exp.TryGetInt64(out var seconds) => DateTimeOffset.FromUnixTimeSeconds(seconds),
            JsonValueKind.String when long.TryParse(exp.GetString(), out var seconds) => DateTimeOffset.FromUnixTimeSeconds(seconds),
            _ => null
        };
    }

    public static bool IsExpired(string? accessToken, TimeSpan skew)
    {
        var expiry = GetExpiry(accessToken);

        return expiry is null || expiry.Value - skew <= DateTimeOffset.UtcNow;
    }

    private static JsonDocument? ReadPayload(string? accessToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken)) return null;

        var segments = accessToken.Split('.');

        if (segments.Length < 2) return null;

        try
        {
            return JsonDocument.Parse(DecodeBase64Url(segments[1]));
        }
        catch (Exception ex) when (ex is FormatException or JsonException)
        {
            return null;
        }
    }

    private static byte[] DecodeBase64Url(string segment)
    {
        var value = segment.Replace('-', '+').Replace('_', '/');

        return Convert.FromBase64String(value.PadRight(value.Length + (4 - value.Length % 4) % 4, '='));
    }
}
