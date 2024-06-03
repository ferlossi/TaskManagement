using System.Text;
using TaskManagement.Services;

namespace TaskManagement.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserService _userService;

        public AuthenticationMiddleware(RequestDelegate next, IUserService userService)
        {
            _next = next;
            _userService = userService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                var username = context.Request.Headers["Username"].FirstOrDefault();
                var password = context.Request.Headers["Password"].FirstOrDefault();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Username and Password headers are required.");
                    return;
                }

                var user = await _userService.AuthenticateUserAsync(username, password);
                if (user == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Invalid credentials.");
                    return;
                }

                context.Items["User"] = user;
            }

            await _next(context);
        }
    }

}
