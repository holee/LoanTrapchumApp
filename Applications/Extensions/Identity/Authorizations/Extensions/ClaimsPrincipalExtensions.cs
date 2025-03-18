using System.Numerics;
using System.Security.Claims;

namespace Applications.Extensions.Identity.Authorizations.Extensions
{
    public static class ClaimsPrincipalExtensions
    {

        internal static string UserEmail(this ClaimsPrincipal claimsPrincipal)
        {
            var email = claimsPrincipal?.FindFirstValue(ClaimTypes.Email);
            return email ?? string.Empty;
        }
        internal static string UserName(this ClaimsPrincipal claimsPrincipal)
        {
            var username = claimsPrincipal?.FindFirstValue(ClaimTypes.Name);
            return username ?? string.Empty;
        }
        internal static string UserRole(this ClaimsPrincipal claimsPrincipal)
        {
            var role = claimsPrincipal?.FindFirstValue(ClaimTypes.Role);
            return role ?? string.Empty;
        }
        internal static string UserId(this ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier);

            return userId ?? string.Empty;
        }
        internal static bool IsBranchOpen(this ClaimsPrincipal claimsPrincipal)
        {

            if (claimsPrincipal == null && !claimsPrincipal!.Identity!.IsAuthenticated) return false;

            var status = claimsPrincipal?.FindFirstValue("Status");

            if (status is null || string.IsNullOrEmpty(status)) return false;

            if (status == "0") return false;

            return true;

        }
        internal static int GetBranchId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null && !claimsPrincipal!.Identity!.IsAuthenticated) return 0;

            var branchIdString = claimsPrincipal.FindFirstValue("BranchId");

            if (branchIdString is not null && int.TryParse(branchIdString, out var id))
            {
                return id;
            }
            return 0;
        }

        internal static T ToNumer<T>(this string type) where T : IConvertible, INumber<T>
        {
            var testValue = type.Length == 0 ? "0" : type;

            var value = (T)Convert.ChangeType(testValue, typeof(T));

            return value;
        }

        internal static T ToNumer<T>(this object type) where T : IConvertible, INumber<T>
        {
            var testValue = type.ToString()?.Length == 0 ? "0" : type.ToString()!;

            var value = (T)Convert.ChangeType(testValue, typeof(T));

            return value;
        }


    }
}
