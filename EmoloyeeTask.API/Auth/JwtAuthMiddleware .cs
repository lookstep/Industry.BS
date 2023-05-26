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
        //eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNyc2Etc2hhMjU2IiwidHlwIjoiSldUIn0.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoibG9va3N0ZXBAYmsucnUiLCJqdGkiOiI5NTZiNDVmOS1iYTUzLTQxYjAtYTRiMS01YzY3OTYxNzg2OWYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJuYmYiOjE2ODQ5MTY0NDMsImV4cCI6MTY4NDkxNjUwMywiaXNzIjoiSW5kdXN0cnkuQlMiLCJhdWQiOiJJbmR1c3RyeS5JT1NBcHAifQ.gtSUA9y75xc9Fnj4Gi38PzHtBEA7i3sxK-zAAe2vFgco0XuRnaB1_dGtSSIc4I5c4gGE6xhvqmFGcAoCHqLoU0PVQsMlb8ACkanGUUPqvTaqoSZgc0uaKpx4B9ZZHgy4mcyXXLz2rJN4MLDBJn7eSesi7saS4dMZNVwyMsvO1j1pfyuPJLaS5sgiPvBPThJjQITWnsO2mItZ8YVaTSi3VlVAu-uUmqc9UOLSFTw5ao1GQpseRNxTm7XwSILv03KXWJIqeKbyn5wdrEPvVZjJuwRK5DdmfUKuFodOlneDi6LTLOKQEbLGwZUx9tdRXm_uHl38wC4QaRLOAC8KHbgdhQ
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
