using System.Net;

namespace TestDeviceServices.Source
{
    public class DeviceAuthorization
    {
        public DeviceAuthorization(RequestDelegate next) => _next = next;

        private readonly RequestDelegate _next;

        public async Task InvokeAsync(HttpContext context)
        {
            string deviceToken = context.Request.Query["t"],
                deviceGuid = context.Request.Query["dguid"];

            if (String.IsNullOrEmpty(deviceToken) || String.IsNullOrEmpty(deviceGuid))
            {
                await SendResponseAsync(context, HttpStatusCode.Forbidden, "Auth error!");
                return;
            }

            AppContext? appContext = context.RequestServices.GetService<AppContext>();

            if (appContext == null)
            {
                await SendResponseAsync(context, HttpStatusCode.InternalServerError, String.Empty);
                return;
            }

            bool isExists = appContext.DevicesTokens.Any(data => data.Token == deviceToken && data.DevicetGuid == Guid.Parse(deviceGuid));

            if (isExists == false)
            {
                await SendResponseAsync(context, HttpStatusCode.Forbidden, "Auth error!");
                return;
            }

            await _next.Invoke(context);
        }

        private async Task SendResponseAsync(HttpContext context, HttpStatusCode httpStatus, string message)
        {
            context.Response.StatusCode = (int)httpStatus;
            await context.Response.WriteAsync(message);
        }
    }
    public static class DeviceExtension
    {        
        public static IApplicationBuilder UseDeviceValidate(this IApplicationBuilder builder) => 
            builder.UseMiddleware<DeviceAuthorization>();
    }
}
