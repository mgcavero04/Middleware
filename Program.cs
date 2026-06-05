//Middleware for logging, error handling, and authentication integrated into the User Management API and a middleware pipeline optimized for performance and security.
/*
Logs:
-HTTP method
-Request path
-Response status code

✔ Handles errors:
-Catches ALL unhandled exceptions
-Returns JSON:
{
  "error": "Internal server error."
}   
Authenticates:
-Reads Authorization header
-Validates token
-Returns 401 Unauthorized if missing or invalid

✔ Correct middleware order:
-Error handling
-Authentication
-Logging
*/
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();
// ------------------------------------------------------------
// STEP 3: Error‑Handling Middleware (FIRST)
// ------------------------------------------------------------
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        var logger = context.RequestServices
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("ErrorHandling");

        logger.LogError(ex, "Unhandled exception occurred");

        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            error = "Internal server error."
        });
    }
});


// ------------------------------------------------------------
// STEP 4: Authentication Middleware (SECOND)
// ------------------------------------------------------------
app.Use(async (context, next) =>
{
    // Expect header: Authorization: Bearer my-secret-token
    if (!context.Request.Headers.TryGetValue("Authorization", out var headerValue))
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Missing Authorization header");
        return;
    }
    var token = headerValue.ToString().Replace("Bearer ", "");
    const string validToken = "my-secret-token";
    if (token != validToken)
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Invalid token");
        return;
    }
    await next();
});


// ------------------------------------------------------------
// STEP 2: Logging Middleware (LAST)
// ------------------------------------------------------------
app.Use(async (context, next) =>
{
    var logger = context.RequestServices
        .GetRequiredService<ILoggerFactory>()
        .CreateLogger("RequestLogger");

    var method = context.Request.Method;
    var path = context.Request.Path;

    await next();

    var statusCode = context.Response.StatusCode;

    logger.LogInformation("HTTP {method} {path} responded {statusCode}",
        method, path, statusCode);
});


// ------------------------------------------------------------
// Controllers
// ------------------------------------------------------------
app.MapControllers();
app.Run();
