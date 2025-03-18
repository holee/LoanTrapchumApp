using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace TrapchumApp
{
    public class MinimalClaimsTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = (ClaimsIdentity)principal.Identity!;

            // Keep only essential claims
            var essentialClaims = new[]
            {
               ClaimTypes.NameIdentifier,
               ClaimTypes.Name,
               ClaimTypes.Email,
               ClaimTypes.Role
           };

            var claims = identity.Claims
                .Where(c => essentialClaims.Contains(c.Type))
                .ToList();

            var newIdentity = new ClaimsIdentity(
                claims,
                identity.AuthenticationType,
                ClaimTypes.Name,
                ClaimTypes.Role);

            return Task.FromResult(new ClaimsPrincipal(newIdentity));
        }
    }
}
