using BookkeeperRest.New.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookkeeperRest.New.Filters;

public class PasswordAuthAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Allow access if demo mode - no password required for demo
        if ((Environment.GetEnvironmentVariable("ASPNETCORE_IS_DEMO") ?? "false") == "true")
        {
          await next();
        }

        // Refuse access if no password is included in request.
        if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var password))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Refuse access if password is included but is wrong.
        IPasswordService passwordService = context.HttpContext.RequestServices.GetRequiredService<IPasswordService>();

        if (passwordService.DoesPasswordMatch(password) == false)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Allow access if password matches.
        await next();
    }
}