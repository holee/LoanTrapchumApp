using Microsoft.AspNetCore.Mvc;

namespace TrapchumApp.Controllers;

[Microsoft.AspNetCore.Mvc.Route("[controller]/[action]")]
public class CultureController : Controller
{
    public IActionResult SetCulture(string culture, string redirectionUri)
    {
        if (culture != null)
        {
            HttpContext.Response.Cookies.Append(
                Microsoft.AspNetCore.Localization.CookieRequestCultureProvider.DefaultCookieName,
                Microsoft.AspNetCore.Localization.CookieRequestCultureProvider.MakeCookieValue(
                    new Microsoft.AspNetCore.Localization.RequestCulture(culture)));
        }

        return LocalRedirect(redirectionUri);
    }
}
