using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EmoloyeeTask.API.Auth
{
    public class JwtAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Equals("/api/Auth/token", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            var token = context.Request.Headers["Authorization"].FirstOrDefault() ?? string.Empty;

            if (!string.IsNullOrEmpty(token))
            {
                token = token.Split(" ").LastOrDefault() ?? Guid.NewGuid().ToString();
                var tokenHeader = new JwtSecurityTokenHandler();

                var validateParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = AuthOptions.PublicKey,
                    ValidIssuer = AuthOptions.Issuer,
                    ValidAudience = AuthOptions.Audience,
                    ValidateAudience = true,
                    ValidateIssuer = true
                };

                tokenHeader.ValidateToken(token, validateParameters, out SecurityToken securityToken);

                var jwtToken = (JwtSecurityToken)securityToken;

                // Создайте ClaimsIdentity из токена
                var identity = new ClaimsIdentity(jwtToken.Claims, "Bearer");
                // Создайте ClaimsPrincipal и установите его в контексте запроса
                var principal = new ClaimsPrincipal(identity);
                context.User = principal;
            }

            await _next(context);
        }
    }
}
