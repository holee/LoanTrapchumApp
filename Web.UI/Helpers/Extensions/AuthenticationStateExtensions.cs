using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Web.UI.Helpers.Extensions
{
    public static class AuthenticationStateExtensions
    {
        public static string? GetUserId(this AuthenticationState state)
        {
             if(state.User.Identity?.IsAuthenticated == false) return string.Empty;

             return state.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        public static string? GetUserName(this AuthenticationState state) 
        {
            if (state.User.Identity?.IsAuthenticated == false) return string.Empty;

            return state.User?.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        }
    }
}
