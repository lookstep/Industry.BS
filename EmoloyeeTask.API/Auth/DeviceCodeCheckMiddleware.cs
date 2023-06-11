using EmoloyeeTask.Data.Services;

namespace EmoloyeeTask.API.Auth
{
    public class DeviceCodeCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<DeviceCodeCheckMiddleware> _logger;

        public DeviceCodeCheckMiddleware(RequestDelegate next, ILogger<DeviceCodeCheckMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, CachedDeviceCodeService deviceCodeService)
        {
            try
            {
                //TODO: Добавить роль адимн и если роль авторизованного соответсвет админу, то игнорировать данный мидлваре
                if (context.Request.Path.Equals("/api/Auth/token", StringComparison.OrdinalIgnoreCase))
                {
                    await _next(context);
                    return;
                }

                var allowedDeviceCodes = await deviceCodeService.GetAllowedDeviceCode();

                var deviceCodeInRequest = context.Request.Headers["DeviceCode"].FirstOrDefault();

                if (deviceCodeInRequest == null || !allowedDeviceCodes.Any(dc => dc.DeviceCode == deviceCodeInRequest))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    _logger.LogError($"Ваш код устройства: {deviceCodeInRequest}");

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync($"Ваш код устройства пустой или не поддерживается");
                    return;
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync($"Ошибка: {ex.Message}");
            }
        }

    }


}
