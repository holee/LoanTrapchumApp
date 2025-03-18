using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace Applications.Extensions;

//[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
//public class ExceptionFilterAttribute:Attribute
//{

//    private JsonSerializerSettings _settings = new JsonSerializerSettings
//    {
//        TypeNameHandling = TypeNameHandling.Objects,
//        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
//    };
//}
//public override Task OnExceptionAsync(ExceptionContext context)
//{

//    switch (context.Exception)
//    {
//        case WeatherUnavailableException:
//            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.FailedDependency;
//            break;

//        default:
//            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//            break;
//    }

//    context.HttpContext.Response.ContentType = "application/json";
//    return context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(context.Exception, _settings));
//}