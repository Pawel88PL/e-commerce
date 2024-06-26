using backend.Interfaces;

namespace backend.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public TokenValidationMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
                    if (await tokenService.IsTokenRevokedAsync(token))
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }
                }
            }
            await _next(context);
        }
    }
}
