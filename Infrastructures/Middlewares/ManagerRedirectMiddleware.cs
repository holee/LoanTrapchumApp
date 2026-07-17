using Applications.Extensions.Identity.Enumerations;
using Microsoft.AspNetCore.Http;
using IMiddleware = Microsoft.AspNetCore.Http.IMiddleware;

namespace Infrastructures.Middlewares
{
    /// <summary>
    /// Redirects authenticated users who are in the "Manager" role
    /// away from the default landing page to the manager dashboard.
    /// </summary>
    public sealed class ManagerRedirectMiddleware : IMiddleware
    {
        private const string ManagerLandingPath = "/App/Loans/Managers";

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var isRootRequest = context.Request.Path == "/" || context.Request.Path == "";

            if (isRootRequest
                && context.User.Identity is { IsAuthenticated: true }
                && context.User.IsInRole(AppRoles.Manager))
            {
                context.Response.Redirect(ManagerLandingPath);
                return;
            }

            await next(context);
        }
    }
}
