// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Users;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class UserAuthMiddleware {
    public static async Task Use(HttpContext context, Func<Task> next) {
        PathString path = context.Request.Path;

        // Allow signup and static/framework resources
        bool isSignup = path.StartsWithSegments("/signup", StringComparison.OrdinalIgnoreCase);
        bool isFramework = path.StartsWithSegments("/_framework")
            || path.StartsWithSegments("/_content")
            || path.StartsWithSegments("/css")
            || path.StartsWithSegments("/js")
            || path.StartsWithSegments("/favicon")
            || path.StartsWithSegments("/assets")
            || path.StartsWithSegments("/swagger")
            || path.StartsWithSegments("/swagger.json")
            || path.StartsWithSegments("/api")
            || path.StartsWithSegments("/_blazor");

        if (isSignup || isFramework) {
            await next();
            return;
        }

        await using AsyncServiceScope scope = context.RequestServices.CreateAsyncScope();
        var onboarding = scope.ServiceProvider.GetRequiredService<UserOnboarding>();
        var authService = scope.ServiceProvider.GetRequiredService<IUserAuthentication>();
        
        // Check if we need onboarding (no users exist)
        bool needsOnboarding = await onboarding.IsOnboardingRequiredAsync(context.RequestAborted);
        if (needsOnboarding) {
            context.Response.Redirect("/signup");
            return;
        }

        // Check if user is already authenticated
        bool isCorrectUserSignIn = await onboarding.IsCorrectUserSignedIn(context.User, context.RequestAborted);
        if (isCorrectUserSignIn) {
            await next();
            return;
        }

        // User exists but not authenticated - auto sign in single user
        await authService.SignOutAsync();
        bool autoSignedIn = await onboarding.TryAutoSignInSingleUserAsync(authService, context.RequestAborted);
        if (autoSignedIn) {
            // Redirect to the same page to refresh with authenticated context
            context.Response.Redirect(context.Request.Path);
            return;
        }

        await next();
    }
}
