using Microsoft.JSInterop;

namespace Applications.Extensions
{
    public static class JsExtensions
    {
        public static async ValueTask InitialInteractivityTime<T>(this IJSRuntime js,
            DotNetObjectReference<T> objectReference) where T : class
        {
            await js.InvokeVoidAsync("initialInteractivityTime", objectReference);
        }


    }
}
