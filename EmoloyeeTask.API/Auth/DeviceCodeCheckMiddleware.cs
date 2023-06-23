using Industry.BS.Data.Interfaces;
using Industry.BS.Data.Repositories;

namespace EmoloyeeTask.API.Auth
{
    public class DeviceCodeCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IAllowedCashedDeviceCodeRepository> _logger;

        public DeviceCodeCheckMiddleware(RequestDelegate next, ILogger<IAllowedCashedDeviceCodeRepository> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IDeviceCodeRepository deviceCodeService)
        {
            try
            {
                //TODO: Добавить роль адимн и если роль авторизованного соответсвет админу, то игнорировать данный мидлваре
                if (context.Request.Path.Equals("/api/Auth/token", StringComparison.OrdinalIgnoreCase))
                {
                    await _next(context);
                    return;
                }

                var allowedDeviceCodes = await deviceCodeService.GetAllAllowedDeviceCodes();

                var deviceCodeInRequest = context.Request.Headers["DeviceCode"].FirstOrDefault();

                if (deviceCodeInRequest == null || !allowedDeviceCodes.Any(dc => dc.DeviceCode == deviceCodeInRequest))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync($"Ваш код устройства пустой или не поддерживается");
                    return;
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync($"Ошибка: {ex.Message}");
            }
        }

    }


}
