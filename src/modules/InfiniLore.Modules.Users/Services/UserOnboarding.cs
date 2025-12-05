// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Core.Database;
using InfiniLore.Core.Pagination;
using InfiniLore.Modules.Users.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Users;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<UserOnboarding>]
public class UserOnboarding(IReadonlyUnitOfWorkFactory<InfiniLoreDb> readonlyUnitOfWorkFactory) {
    public static async Task Middleware(HttpContext context, Func<Task> next) {
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
        
        // Check if we need onboarding (no users exist)
        bool needsOnboarding = await onboarding.IsOnboardingRequiredAsync(context.RequestAborted);
        if (needsOnboarding) {
            context.Response.Redirect("/signup");
            return;
        }

        // Check if user is already authenticated
        if (context.User.Identity?.IsAuthenticated == true) {
            await next();
            return;
        }

        // User exists but not authenticated - auto sign in single user
        var authService = scope.ServiceProvider.GetRequiredService<UserAuthentication>();
        bool autoSignedIn = await onboarding.TryAutoSignInSingleUserAsync(authService, context.RequestAborted);

        if (autoSignedIn) {
            // Redirect to the same page to refresh with authenticated context
            context.Response.Redirect(context.Request.Path);
            return;
        }

        await next();
    }

    private async Task<bool> IsOnboardingRequiredAsync(CancellationToken ct = default) {
        await using IReadonlyUnitOfWork<InfiniLoreDb> uow = readonlyUnitOfWorkFactory.Create();
        var repo = await uow.GetRepositoryAsync<UserModelRepository>(ct);
        bool any = await repo.AnyAsync(ct: ct);
        return !any;
    }

    private async Task<bool> TryAutoSignInSingleUserAsync(UserAuthentication authService, CancellationToken ct = default) {
        await using IReadonlyUnitOfWork<InfiniLoreDb> uow = readonlyUnitOfWorkFactory.Create();
        var repo = await uow.GetRepositoryAsync<UserModelRepository>(ct);

        // This only works if there's exactly one user
        PaginatedData<UserModel> users = await repo.GetAllAsync(PaginationData.Default, ct: ct);

        // Only auto sign-in if there's exactly one user
        if (users.TotalCount != 1) {
            return false;
        }

        UserModel user = users.Items.First();
        return await authService.SignInAsync(user);
    }
}
