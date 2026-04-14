using HRMSAPI.Data;
using HRMSAPI.Models.Entities;
using System.Security.Claims;

namespace HRMSAPI.Middleware;

public class AuditLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public AuditLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
    {
        await _next(context);

        if (context.Request.Method != "GET")
        {
            var userIdString = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid? userId = string.IsNullOrEmpty(userIdString) ? null : Guid.Parse(userIdString);

            var auditLog = new AuditLog
            {
                UserId = userId,
                Action = context.Request.Method,
                Module = context.Request.Path,
                Timestamp = DateTime.UtcNow,
                IPAddress = context.Connection.RemoteIpAddress?.ToString()
            };

            dbContext.AuditLogs.Add(auditLog);
            await dbContext.SaveChangesAsync();
        }
    }
}