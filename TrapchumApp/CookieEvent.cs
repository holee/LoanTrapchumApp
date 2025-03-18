using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TrapchumApp
{
    public class CookieEvent : CookieAuthenticationEvents
    {
        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            context.RedirectUri = "/Account/Login";
            return base.RedirectToLogin(context);
        }
    }
}
